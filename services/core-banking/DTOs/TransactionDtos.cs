using System.ComponentModel.DataAnnotations;

namespace BIK.CoreBanking.DTOs
{
    public class SingleTransactionDto
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser estrictamente mayor a cero.")]
        public decimal Amount { get; set; }
    }

    public class TransferDto
    {
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto a transferir debe ser estrictamente mayor a cero.")]
        public decimal Amount { get; set; }
    }
}