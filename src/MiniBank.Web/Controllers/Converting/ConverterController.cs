using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Enums;

namespace MiniBank.Web.Controllers.Converting
{
    [ApiController]
    [Authorize]
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
        public async Task<double> ConvertOneCurrencyToAnother(int amount, Currencies fromCurrency,
            Currencies toCurrency)
        {
            var convertedSum = await _currencyConverter.ConvertCurrency(amount, fromCurrency, toCurrency);
            return convertedSum;
        }
    }
}