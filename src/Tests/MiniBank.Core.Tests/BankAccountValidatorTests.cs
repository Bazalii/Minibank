using System;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Core.Domains.BankAccounts.Validators;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Enums;
using Moq;
using Xunit;

namespace MiniBank.Core.Tests;

public class BankAccountValidatorTests
{
    private readonly AbstractValidator<BankAccount> _bankAccountValidator;

    private readonly Mock<IUserRepository> _mockUserRepository;

    public BankAccountValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _bankAccountValidator = new BankAccountValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task ValidateAndThrowAsync_AccountWithNegativeAmountOfMoney_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            UserId = userId,
            AmountOfMoney = -100
        };

        _mockUserRepository
            .Setup(repository => repository.Exists(userId, default))
            .ReturnsAsync(true);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountValidator.ValidateAndThrowAsync(bankAccount));

        // ASSERT
        Assert.Contains("AmountOfMoney: should be greater than 0!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndThrowAsync_AccountWithUserThatNotExists_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            UserId = userId,
            AmountOfMoney = 100
        };

        _mockUserRepository
            .Setup(repository => repository.Exists(userId, default))
            .ReturnsAsync(false);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountValidator.ValidateAndThrowAsync(bankAccount));

        // ASSERT
        Assert.Contains("UserId: user with entered Id doesn't exist!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_SuccessPath_UserIsValidated()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            UserId = userId,
            AmountOfMoney = 100,
            CurrencyCode = Currencies.EUR
        };

        _mockUserRepository
            .Setup(repository => repository.Exists(userId, default))
            .ReturnsAsync(true);

        // ACT
        await _bankAccountValidator.ValidateAndThrowAsync(bankAccount);

        // ASSERT
        _mockUserRepository
            .Verify(repository => repository.Exists(bankAccount.UserId, default), Times.Once());
    }
}