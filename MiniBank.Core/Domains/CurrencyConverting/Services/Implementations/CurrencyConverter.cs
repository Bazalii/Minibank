using MiniBank.Core.Exceptions;

namespace MiniBank.Core.Domains.CurrencyConverting.Services.Implementations
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public CurrencyConverter(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public double ConvertCurrency(int amount, string currencyName)
        {
            var result = amount * _exchangeRateProvider.GetCourse(currencyName);
            return result >= 0 ? result : throw new UserFriendlyException("Incorrect amount of rubles or currency name!");
        }
    }
}