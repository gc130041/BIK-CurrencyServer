using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIK.CoreBanking.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Balance { get; set; }

        public int UserId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}