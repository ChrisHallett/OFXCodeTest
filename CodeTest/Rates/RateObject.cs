namespace CodeTest.Rates
{
    public class RateObject
    {
        public string Base { get; set; }
        public Dictionary<string, double> Rates { get; set; } = new Dictionary<string, double>();
    }
}
