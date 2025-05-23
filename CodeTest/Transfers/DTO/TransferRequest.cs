using System.ComponentModel.DataAnnotations;

namespace CodeTest.Transfers.DTO
{
    public class TransferRequest
    {
        [Required]
        public Guid QuoteId { get; set; }
        [Required]
        public Payer Payer { get; set; }
        [Required]
        public Recipient Recipient { get; set; }
    }
}
