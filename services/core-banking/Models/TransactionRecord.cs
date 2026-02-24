using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIK.CoreBanking.Models
{
    public class TransactionRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string TransactionType { get; set; } = string.Empty; // Ej: "Deposit", "Withdrawal", "TransferIn", "TransferOut"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("AccountId")]
        public Account Account { get; set; } = null!;
    }
}