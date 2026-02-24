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
        public string ActionType { get; set; } = string.Empty; // Ej: "Transaction", "LoginAttempt", "SuspiciousActivity"

        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; } = string.Empty; // Detalles

        public int? AccountId { get; set; } // Puede ser null si la actividad no está ligada a una cuenta específica

        public int? UserId { get; set; } // Puede ser null si es un intento de acceso no identificado

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Severity { get; set; } = "Info"; // "Info", "Warning", "Critical"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "varchar(50)")]
        public string SourceIpAddress { get; set; } = string.Empty; // Útil para rastrear actividad sospechosa
    }
}