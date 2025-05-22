namespace CodeTest.Helpers
{
    public static class CurrencyParser
    {

        public static Currency ParseCurrency(string input)
        {
            var parsedInput = input.ToLowerInvariant();
            switch (parsedInput)
            {
                case "aud":
                {
                    return Currency.AUD;
                }
                case "usd":
                {
                    return Currency.USD;
                }
                case "eur":
                {
                    return Currency.EUR;
                }
                case "inr":
                {
                    return Currency.INR;
                }
                case "php":
                {
                    return Currency.PHP;
                }
                default:
                {
                    throw new ArgumentException("Currency not supported");
                }
            }
        }
    }
}
