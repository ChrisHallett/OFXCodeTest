namespace CodeTest.Transfers.DTO
{
    public class TransferResponse
    {
        public Guid TransferId { get; set; }
        public string Status { get; set; }
        public TransferDetails TransferDetails { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }
}
