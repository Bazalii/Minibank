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

        public double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("The amount of money cannot be negative!");
            }

            return amount * _exchangeRateProvider.GetCourse(fromCurrency) / _exchangeRateProvider.GetCourse(toCurrency);
        }
    }
}