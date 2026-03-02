using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIK.AuditService.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ActionType { get; set; } = string.Empty; 

        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; } = string.Empty; 

        public int? AccountId { get; set; } 

        [Column(TypeName = "varchar(24)")]
        public string? UserId { get; set; } 

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Severity { get; set; } = "Info"; 

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "varchar(50)")]
        public string SourceIpAddress { get; set; } = string.Empty; 
    }
}