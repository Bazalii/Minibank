using System.Threading.Tasks;

namespace MiniBank.Core.Domains.CurrencyConverting.Services
{
    public interface IExchangeRateProvider
    {
        Task<double> GetCourse(string currencyCode);
    }
}