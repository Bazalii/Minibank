using System.Threading.Tasks;
using MiniBank.Core.Enums;

namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface ICurrencyConverter
    {
        Task<double> ConvertCurrency(double amount, Currencies fromCurrency, Currencies toCurrency);
    }
}