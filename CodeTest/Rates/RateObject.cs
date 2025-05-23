namespace CodeTest.Rates
{
    public class RateObject
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}
