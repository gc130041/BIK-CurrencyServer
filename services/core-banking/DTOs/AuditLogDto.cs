namespace BIK.CoreBanking.DTOs
{
    public class AuditLogDto
    {
        public string ActionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? AccountId { get; set; }
        public string Severity { get; set; } = "Info";
        public string SourceIpAddress { get; set; } = "127.0.0.1";
    }
}