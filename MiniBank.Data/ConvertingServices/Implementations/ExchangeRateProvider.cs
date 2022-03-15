using System;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Exceptions;

namespace MiniBank.Data.ConvertingServices.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly double _minValue;

        private readonly double _maxValue;

        private readonly Random _randomizer;

        public ExchangeRateProvider(double minValue, double maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _randomizer = new Random();
        }
        
        public double GetCourse(string currencyName)
        {
            return currencyName switch
            {
                "Dollar" => _randomizer.NextDouble() * (_maxValue - _minValue) + _minValue,
                "Euro" => _randomizer.NextDouble() * (_maxValue - _minValue) + _minValue,
                _ => throw new ValidationException("Incorrect type of currency!")
            };
        }
    }
}