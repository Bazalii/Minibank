using System;
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

        public double ConvertCurrency(double amount, Currencies fromCurrency, Currencies toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("The amount of money cannot be negative!");
            }

            var result =
                Math.Round(
                    amount * _exchangeRateProvider.GetCourse(fromCurrency.ToString()) /
                    _exchangeRateProvider.GetCourse(toCurrency.ToString()), 2);

            return result;
        }
    }
}