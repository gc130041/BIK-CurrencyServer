using System.ComponentModel.DataAnnotations;

namespace BIK.CoreBanking.DTOs
{
    public class SingleTransactionDto
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser estrictamente mayor a cero.")]
        public decimal Amount { get; set; }
    }

    public class TransferDto
    {
        [Required]
        public string FromAccountNumber { get; set; } = string.Empty;

        [Required]
        public string ToAccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto a transferir debe ser estrictamente mayor a cero.")]
        public decimal Amount { get; set; }
    }
}