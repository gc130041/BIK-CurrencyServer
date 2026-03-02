using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BIK.CoreBanking.Interfaces;
using BIK.CoreBanking.DTOs;
using BIK.CoreBanking.Data;

namespace BIK.CoreBanking.Controllers
{
    [ApiController]
    [Route("BIK/v1/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;
        private readonly IAuditServiceClient _auditService;

        public TransactionsController(
            ITransactionService transactionService, 
            ApplicationDbContext context,
            IAuditServiceClient auditService)
        {
            _transactionService = transactionService;
            _context = context;
            _auditService = auditService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] SingleTransactionDto request)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber);
                if (account == null) return NotFound(new { error = "Cuenta no encontrada en el Core Banking" });

                await _transactionService.ProcessDepositAsync(account.Id, request.Amount);

                await _auditService.LogActivityAsync(new AuditLogDto
                {
                    ActionType = "Deposit",
                    Description = $"Depósito en ventanilla de Q{request.Amount}",
                    AccountId = account.Id,
                    UserId = account.UserId,
                    Severity = "Info",
                    SourceIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                });

                return Ok(new { message = "Depósito realizado con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] SingleTransactionDto request)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber);
                if (account == null) return NotFound(new { error = "Cuenta no encontrada en el Core Banking" });

                await _transactionService.ProcessWithdrawalAsync(account.Id, request.Amount);

                await _auditService.LogActivityAsync(new AuditLogDto
                {
                    ActionType = "Withdraw",
                    Description = $"Retiro de Q{request.Amount}",
                    AccountId = account.Id,
                    UserId = account.UserId,
                    Severity = "Warning",
                    SourceIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                });

                return Ok(new { message = "Retiro realizado con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto request)
        {
            try
            {
                var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber);
                var toAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber);

                if (fromAccount == null || toAccount == null) 
                    return NotFound(new { error = "Una o ambas cuentas no existen en el Core Banking" });

                await _transactionService.ProcessTransferAsync(fromAccount.Id, toAccount.Id, request.Amount);

                await _auditService.LogActivityAsync(new AuditLogDto
                {
                    ActionType = "Transfer Out",
                    Description = $"Transferencia enviada de Q{request.Amount} hacia cuenta {toAccount.AccountNumber}",
                    AccountId = fromAccount.Id,
                    UserId = fromAccount.UserId,
                    Severity = "Info",
                    SourceIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                });

                await _auditService.LogActivityAsync(new AuditLogDto
                {
                    ActionType = "Transfer In",
                    Description = $"Transferencia recibida de Q{request.Amount} desde cuenta {fromAccount.AccountNumber}",
                    AccountId = toAccount.Id,
                    UserId = toAccount.UserId,
                    Severity = "Info",
                    SourceIpAddress = "Internal System"
                });

                return Ok(new { message = "Transferencia realizada con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}