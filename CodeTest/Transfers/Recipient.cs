using System.ComponentModel.DataAnnotations;

namespace CodeTest.Transfers
{
    public class Recipient
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string BankName { get; set; }
    }
}
