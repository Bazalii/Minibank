namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface ICurrencyConverter
    {
        double ConvertCurrency(int amount, string currencyName);
    }
}