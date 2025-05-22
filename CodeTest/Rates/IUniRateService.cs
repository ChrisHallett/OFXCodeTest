using CodeTest.Helpers;

namespace CodeTest.Rates
{
    public interface IUniRateService
    {
        Task<double> GetRate(Currency buyCurrency, Currency sellCurrency);
    }
}
