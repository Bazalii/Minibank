using MiniBank.Core.Enums;

namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface ICurrencyConverter
    {
        double ConvertCurrency(double amount, Currencies fromCurrency, Currencies toCurrency);
    }
}