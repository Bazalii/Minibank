namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface IExchangeRateProvider
    {
        double GetCourse(string currencyName);
    }
}