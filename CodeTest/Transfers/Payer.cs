using System.ComponentModel.DataAnnotations;

namespace CodeTest.Transfers
{
    public class Payer
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TransferReason { get; set; }
    }
}
