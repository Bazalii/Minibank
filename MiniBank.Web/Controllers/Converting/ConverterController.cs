using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Enums;

namespace MiniBank.Web.Controllers.Converting
{
    [ApiController]
    [Route("[controller]")]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public ConverterController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        [Route("/convert")]
        public double ConvertOneCurrencyToAnother(int amount, Currencies fromCurrency, Currencies toCurrency)
        {
            return _currencyConverter.ConvertCurrency(amount, fromCurrency, toCurrency);
        }
    }
}