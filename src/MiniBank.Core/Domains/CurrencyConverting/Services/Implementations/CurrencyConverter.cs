using System;
using System.Threading.Tasks;
using MiniBank.Core.Enums;
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

        public async Task<double> ConvertCurrency(double amount, Currencies fromCurrency, Currencies toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("The amount of money cannot be negative!");
            }

            var fromCourse = await _exchangeRateProvider.GetCourse(fromCurrency.ToString());
            var toCourse = await _exchangeRateProvider.GetCourse(toCurrency.ToString());
            var result = Math.Round(amount * fromCourse / toCourse, 2);

            return result;
        }
    }
}