using System.Threading.Tasks;
using BIK.CoreBanking.DTOs;

namespace BIK.CoreBanking.Interfaces
{
    public interface IAuditServiceClient
    {
        Task LogActivityAsync(AuditLogDto log);
    }
}