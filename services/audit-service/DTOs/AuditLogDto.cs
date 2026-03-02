using System.ComponentModel.DataAnnotations;

namespace BIK.AuditService.DTOs
{
    public class AuditLogDto
    {
        [Required]
        public string ActionType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? AccountId { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string Severity { get; set; } = "Info";

        public string SourceIpAddress { get; set; } = string.Empty;
    }
}