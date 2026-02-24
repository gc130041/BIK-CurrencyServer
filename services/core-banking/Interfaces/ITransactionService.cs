using System.Threading.Tasks;

namespace BIK.CoreBanking.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> ProcessDepositAsync(int accountId, decimal amount);
        
        Task<bool> ProcessWithdrawalAsync(int accountId, decimal amount);
        
        Task<bool> ProcessTransferAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}