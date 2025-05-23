namespace CodeTest.Transfers
{
    public class TransferDetails
    {
        public Guid QuoteId { get; set; }
        public Payer Payer { get; set; }
        public Recipient Recipient { get; set; }
    }
}
