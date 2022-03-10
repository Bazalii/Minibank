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
            if (amount < 0)
                throw new UserFriendlyException("The amount of rubles cannot be negative!");
            return amount * _exchangeRateProvider.GetCourse(currencyName);
        }
    }
}