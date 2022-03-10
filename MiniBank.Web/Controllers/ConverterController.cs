using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.CurrencyConverting.Services;

namespace MiniBank.Web.Controllers
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
        public double ConvertRublesToCurrency(int amount, string currencyName)
        {
            return _currencyConverter.ConvertCurrency(amount, currencyName);
        }
    }
}