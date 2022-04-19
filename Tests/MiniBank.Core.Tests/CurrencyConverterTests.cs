using System.Threading.Tasks;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.CurrencyConverting.Services.Implementations;
using MiniBank.Core.Enums;
using MiniBank.Core.Exceptions;
using Moq;
using Xunit;

namespace MiniBank.Core.Tests;

public class CurrencyConverterTests
{
    private readonly ICurrencyConverter _mockCurrencyConverter;

    private readonly Mock<IExchangeRateProvider> _mockExchangeRateProvider;

    public CurrencyConverterTests()
    {
        _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
        _mockExchangeRateProvider
            .Setup(provider => provider.GetCourse("RUB"))
            .ReturnsAsync(1);
        _mockExchangeRateProvider
            .Setup(provider => provider.GetCourse("EUR"))
            .ReturnsAsync(80);
        _mockExchangeRateProvider
            .Setup(provider => provider.GetCourse("USD"))
            .ReturnsAsync(70);

        _mockCurrencyConverter = new CurrencyConverter(_mockExchangeRateProvider.Object);
    }

    [Fact]
    public async Task ConvertCurrency_AmountIsNegative_ThrowException()
    {
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _mockCurrencyConverter.ConvertCurrency(-1, Currencies.EUR, Currencies.RUB));

        Assert.Contains("The amount of money cannot be negative!", exception.Message);
    }

    [Theory]
    [InlineData(1, Currencies.EUR, Currencies.RUB, 80)]
    [InlineData(1, Currencies.RUB, Currencies.RUB, 1)]
    [InlineData(2.35, Currencies.EUR, Currencies.RUB, 188)]
    [InlineData(0.57, Currencies.USD, Currencies.EUR, 0.5)]
    public async Task ConvertCurrency_SuccessPath_ReturnsConvertedResultGetCourseIsCalledTwice(double amount,
        Currencies fromCurrency, Currencies toCurrency, double expectedResult)
    {
        var result = await _mockCurrencyConverter.ConvertCurrency(amount, fromCurrency, toCurrency);

        if (fromCurrency == toCurrency)
        {
            _mockExchangeRateProvider
                .Verify(provider => provider.GetCourse(fromCurrency.ToString()), Times.Exactly(2));
        }
        else
        {
            _mockExchangeRateProvider
                .Verify(provider => provider.GetCourse(fromCurrency.ToString()), Times.Once);
            _mockExchangeRateProvider
                .Verify(provider => provider.GetCourse(toCurrency.ToString()), Times.Once);
        }

        Assert.Equal(expectedResult, result);
    }
}