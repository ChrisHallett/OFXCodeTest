namespace CodeTest.Transfers.DTO
{
    public class QuoteResponse
    {
        public Guid QuoteId { get; set; }
        public decimal OfxRate { get; set; }
        public decimal InverseOfxRate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}
