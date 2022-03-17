namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface ICurrencyConverter
    {
        double ConvertCurrency(double amount, string fromCurrency, string toCurrency);
    }
}