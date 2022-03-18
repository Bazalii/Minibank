using System;
using System.Net.Http;
using System.Net.Http.Json;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Exceptions;
using MiniBank.Data.ConvertingServices.HttpClients.Models;

namespace MiniBank.Data.ConvertingServices.HttpClients.Implementations
{
    public class ExchangeRateHttpProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateHttpProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public double GetCourse(string currencyCode)
        {
            if (currencyCode == "RUB")
                return 1;

            var response = _httpClient.GetFromJsonAsync<ExchangeRateResponse>("daily_json.js")
                .GetAwaiter().GetResult();

            if (!response.Currencies.ContainsKey(currencyCode))
            {
                throw new ValidationException($"{currencyCode} is not correct currency code!");
            }

            var result = Math.Round(response.Currencies[currencyCode].Value, 2);

            return result;
        }
    }
}