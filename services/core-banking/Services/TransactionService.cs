using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BIK.CoreBanking.Data;
using BIK.CoreBanking.Models;
using BIK.CoreBanking.Interfaces;
using BIK.CoreBanking.DTOs;

namespace BIK.CoreBanking.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditServiceClient _auditClient;

        public TransactionService(ApplicationDbContext context, IAuditServiceClient auditClient)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _auditClient = auditClient ?? throw new ArgumentNullException(nameof(auditClient));
            }

        public async Task<bool> ProcessDepositAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new Exception("Cuenta no encontrada.");

            account.Balance += amount;

            var record = new TransactionRecord
            {
                AccountId = accountId,
                Amount = amount,
                TransactionType = "Deposit"
            };

            _context.TransactionRecords.Add(record);
            await _context.SaveChangesAsync();
            
            await _auditClient.LogActivityAsync(new AuditLogDto {
                ActionType = "Deposit",
                Description = $"Depósito de {amount} procesado exitosamente.",
                AccountId = accountId,
                Severity = "Info"
            });

            return true;
        }

        public async Task<bool> ProcessWithdrawalAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new Exception("Cuenta no encontrada.");

            if (account.Balance < amount)
                throw new Exception("Fondos insuficientes.");

            account.Balance -= amount;

            var record = new TransactionRecord
            {
                AccountId = accountId,
                Amount = -amount,
                TransactionType = "Withdrawal"
            };

            _context.TransactionRecords.Add(record);
            await _context.SaveChangesAsync();
            
            await _auditClient.LogActivityAsync(new AuditLogDto {
                ActionType = "Withdrawal",
                Description = $"Retiro de {amount} procesado exitosamente.",
                AccountId = accountId,
                Severity = "Info"
            });

            return true;
        }

        public async Task<bool> ProcessTransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId) 
                throw new Exception("No puedes transferir dinero a la misma cuenta.");

            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
                var toAccount = await _context.Accounts.FindAsync(toAccountId);

                if (fromAccount == null || toAccount == null)
                    throw new Exception("Una o ambas cuentas no existen.");

                if (fromAccount.Balance < amount)
                    throw new Exception("Fondos insuficientes en la cuenta de origen.");

                fromAccount.Balance -= amount;
                toAccount.Balance += amount;

                _context.TransactionRecords.Add(new TransactionRecord
                {
                    AccountId = fromAccountId, Amount = -amount, TransactionType = "TransferOut"
                });

                _context.TransactionRecords.Add(new TransactionRecord
                {
                    AccountId = toAccountId, Amount = amount, TransactionType = "TransferIn"
                });

                await _context.SaveChangesAsync();
                
                await dbTransaction.CommitAsync();

                    await _auditClient.LogActivityAsync(new AuditLogDto {
                    ActionType = "Transfer",
                    Description = $"Transferencia de {amount} desde cuenta {fromAccountId} hacia cuenta {toAccountId}.",
                    Severity = "Info"
                });

                return true;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}