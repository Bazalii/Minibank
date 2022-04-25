using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.BankAccounts.Services;
using MiniBank.Core.Domains.BankAccounts.Services.Implementations;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Data.Exceptions;
using Moq;
using Xunit;
using ValidationException = MiniBank.Core.Exceptions.ValidationException;

namespace MiniBank.Core.Tests;

public class BankAccountServiceTests
{
    private readonly IBankAccountService _bankAccountService;

    private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;

    private readonly Mock<ITransactionRepository> _mockTransactionRepository;

    private readonly Mock<ICurrencyConverter> _mockCurrencyConverter;

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    private readonly Mock<IValidator<BankAccount>> _mockBankAccountValidator;

    public BankAccountServiceTests()
    {
        _mockBankAccountRepository = new Mock<IBankAccountRepository>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockCurrencyConverter = new Mock<ICurrencyConverter>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockBankAccountValidator = new Mock<IValidator<BankAccount>>();

        _bankAccountService = new BankAccountService(_mockBankAccountRepository.Object, _mockCurrencyConverter.Object,
            _mockTransactionRepository.Object, _mockUnitOfWork.Object, _mockBankAccountValidator.Object);
    }

    [Fact]
    public async Task Add_SuccessPath_AccountIsValidatedAddInBankAccountRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var bankAccountCreationModel = new BankAccountCreationModel
        {
            UserId = userId
        };

        // ACT
        await _bankAccountService.Add(bankAccountCreationModel, default);

        // ASSERT
        _mockBankAccountValidator
            .Verify(validator => validator.ValidateAsync(It.IsAny<ValidationContext<BankAccount>>(), default),
                Times.Once());
        _mockBankAccountRepository
            .Verify(repository => repository.Add(It.Is<BankAccount>(account => account.UserId == userId), default),
                Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once());
    }

    [Fact]
    public async Task GetById_BankAccountNotExists_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(accountId, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {accountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.GetById(accountId, default));

        // ASSERT
        Assert.Equal($"Account with id: {accountId} is not found!", exception.Message);
    }

    [Fact]
    public async Task GetById_SuccessPath_GetByIdInBankAccountRepositoryIsCalledReturnsCorrespondingBankAccount()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId
        };

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(accountId, default))
            .ReturnsAsync(bankAccount);

        // ACT
        var result = await _bankAccountService.GetById(accountId, default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.GetById(accountId, default), Times.Once());

        Assert.Equal(bankAccount, result);
    }

    [Theory]
    [MemberData(nameof(GetDataForGetAllTests))]
    public async Task GetAll_SuccessPath_GetAllInBankAccountRepositoryIsCalledReturnsIEnumerableOfBankAccounts(
        IEnumerable<BankAccount> expectedResult)
    {
        // ARRANGE
        var enumerable = expectedResult.ToList();
        _mockBankAccountRepository
            .Setup(repository => repository.GetAll(default))
            .ReturnsAsync(enumerable);

        // ACT
        var bankAccounts = await _bankAccountService.GetAll(default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.GetAll(default), Times.Once());

        Assert.Equal(enumerable, bankAccounts);
    }

    [Fact]
    public async Task Update_BankAccountNotExists_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId
        };

        _mockBankAccountRepository
            .Setup(repository => repository.Update(bankAccount, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {accountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.Update(bankAccount, default));

        // ASSERT
        Assert.Equal($"Account with id: {accountId} is not found!", exception.Message);
    }

    [Fact]
    public async Task
        Update_SuccessPath_AccountIsValidatedUpdateInBankAccountRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId
        };

        // ACT
        await _bankAccountService.Update(bankAccount, default);

        // ASSERT
        _mockBankAccountValidator
            .Verify(validator => validator.ValidateAsync(It.IsAny<ValidationContext<BankAccount>>(), default),
                Times.Once());
        _mockBankAccountRepository
            .Verify(repository => repository.Update(bankAccount, default), Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once());
    }

    [Fact]
    public async Task UpdateAccountMoney_BankAccountNotExists_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.UpdateAccountMoney(accountId, 100, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {accountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.UpdateAccountMoney(accountId, 100, default));

        // ASSERT
        Assert.Equal($"Account with id: {accountId} is not found!", exception.Message);
    }

    [Fact]
    public async Task
        UpdateAccountMoney_SuccessPath_UpdateAccountMoneyInBankAccountRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        // ACT
        await _bankAccountService.UpdateAccountMoney(accountId, 100, default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.UpdateAccountMoney(accountId, 100, default), Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once);
    }

    [Fact]
    public async Task CloseAccountById_ThereIsMoneyOnBankAccount_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId,
            AmountOfMoney = 100
        };

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(accountId, default))
            .ReturnsAsync(bankAccount);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountService.CloseAccountById(accountId, default));

        // ASSERT
        Assert.Equal($"Amount of money on account with id: {accountId} that you want to close should be 0!",
            exception.Message);
    }

    [Fact]
    public async Task
        CloseAccountById_SuccessPath_GetByIdAndUpdateInBankAccountRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId,
            AmountOfMoney = 0,
            IsOpened = true
        };

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(accountId, default))
            .ReturnsAsync(bankAccount);

        // ACT
        await _bankAccountService.CloseAccountById(accountId, default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.GetById(accountId, default), Times.Once);
        _mockBankAccountRepository
            .Verify(repository => repository.Update(It.Is<BankAccount>(account => account.Id == accountId), default));
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once);

        Assert.False(bankAccount.IsOpened);
    }

    [Fact]
    public async Task CalculateCommission_WithdrawalAccountNotExists_ThrowException()
    {
        // ARRANGE
        var withdrawalAccountId = Guid.NewGuid();

        var replenishmentAccountId = Guid.NewGuid();

        const int amount = 100;

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(withdrawalAccountId, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {withdrawalAccountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.CalculateCommission(amount, withdrawalAccountId, replenishmentAccountId,
                default));

        // ASSERT
        Assert.Equal($"Account with id: {withdrawalAccountId} is not found!", exception.Message);
    }

    [Fact]
    public async Task CalculateCommission_ReplenishmentAccountNotExists_ThrowException()
    {
        // ARRANGE
        var withdrawalAccountId = Guid.NewGuid();

        var replenishmentAccountId = Guid.NewGuid();

        const int amount = 100;

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(replenishmentAccountId, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {replenishmentAccountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.CalculateCommission(amount, withdrawalAccountId, replenishmentAccountId,
                default));

        // ASSERT
        Assert.Equal($"Account with id: {replenishmentAccountId} is not found!", exception.Message);
    }

    [Theory]
    [MemberData(nameof(GetDataForCalculateCommissionTests))]
    public async Task CalculateCommission_SuccessPath_CommissionIsCalculated(double amount,
        BankAccount withdrawalAccount, BankAccount replenishmentAccount, double expectedCommission)
    {
        // ARRANGE
        _mockBankAccountRepository
            .Setup(repository => repository.GetById(withdrawalAccount.Id, default))
            .ReturnsAsync(withdrawalAccount);
        _mockBankAccountRepository
            .Setup(repository => repository.GetById(replenishmentAccount.Id, default))
            .ReturnsAsync(replenishmentAccount);

        // ACT
        var commission =
            await _bankAccountService.CalculateCommission(amount, withdrawalAccount.Id, replenishmentAccount.Id,
                default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.GetById(withdrawalAccount.Id, default), Times.Once);
        _mockBankAccountRepository
            .Verify(repository => repository.GetById(replenishmentAccount.Id, default), Times.Once);

        Assert.Equal(expectedCommission, commission);
    }


    [Fact]
    public async Task TransferMoney_AccountsAreTheSame_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        var bankAccount = new BankAccount
        {
            Id = accountId
        };

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(accountId, default))
            .ReturnsAsync(bankAccount);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountService.TransferMoney(100, accountId, accountId, default));

        // ASSERT
        Assert.Equal("Money can be transferred only between different accounts!", exception.Message);
    }

    [Fact]
    public async Task TransferMoney_NotEnoughMoneyOnWithdrawalAccount_ThrowException()
    {
        // ARRANGE
        var withdrawalAccountId = Guid.NewGuid();

        var replenishmentAccountId = Guid.NewGuid();

        var withdrawalAccount = new BankAccount
        {
            Id = withdrawalAccountId,
            AmountOfMoney = 50
        };

        var replenishmentAccount = new BankAccount
        {
            Id = replenishmentAccountId,
            AmountOfMoney = 10
        };

        _mockBankAccountRepository
            .Setup(repository => repository.GetById(withdrawalAccountId, default))
            .ReturnsAsync(withdrawalAccount);
        _mockBankAccountRepository
            .Setup(repository => repository.GetById(replenishmentAccountId, default))
            .ReturnsAsync(replenishmentAccount);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountService.TransferMoney(100, withdrawalAccountId, replenishmentAccountId, default));

        // ASSERT
        Assert.Equal("Not enough money to transfer!", exception.Message);
    }

    [Theory]
    [MemberData(nameof(GetDataForTransferMoneyTest))]
    public async Task TransferMoney_SuccessPath_MoneyIsWithdrawnFromWithdrawalAccountReplenishmentAccountIsReplenished(
        BankAccount withdrawalAccount, BankAccount replenishmentAccount, double amount)
    {
        // ARRANGE
        _mockBankAccountRepository
            .Setup(repository => repository.GetById(withdrawalAccount.Id, default))
            .ReturnsAsync(withdrawalAccount);
        _mockBankAccountRepository
            .Setup(repository => repository.GetById(replenishmentAccount.Id, default))
            .ReturnsAsync(replenishmentAccount);

        // ACT 
        var commission =
            await _bankAccountService.CalculateCommission(amount, withdrawalAccount.Id, replenishmentAccount.Id,
                default);
        var finalWithdrawalAccountAmount = withdrawalAccount.AmountOfMoney - amount;
        var finalReplenishmentAccountAmount = replenishmentAccount.AmountOfMoney + amount - commission;

        await _bankAccountService.TransferMoney(amount, withdrawalAccount.Id, replenishmentAccount.Id, default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(
                repository =>
                    repository.UpdateAccountMoney(withdrawalAccount.Id, finalWithdrawalAccountAmount, default),
                Times.Once());
        _mockBankAccountRepository
            .Verify(
                repository =>
                    repository.UpdateAccountMoney(replenishmentAccount.Id, finalReplenishmentAccountAmount, default),
                Times.Once());
        _mockTransactionRepository
            .Verify(
                repository =>
                    repository.Add(
                        It.Is<Transaction>(transaction =>
                            transaction.WithdrawalAccount == withdrawalAccount.Id &&
                            transaction.ReplenishmentAccount == replenishmentAccount.Id &&
                            transaction.AmountOfMoney == amount - commission),
                        default), Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once);
    }

    [Fact]
    public async Task DeleteById_AccountNotExists_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.DeleteById(accountId, default))
            .ThrowsAsync(new ObjectNotFoundException($"Account with id: {accountId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _bankAccountService.DeleteById(accountId, default));

        // ASSERT
        Assert.Equal($"Account with id: {accountId} is not found!", exception.Message);
    }

    [Fact]
    public async Task DeleteById_AccountIsNotClosed_ThrowException()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.IsOpened(accountId, default))
            .ReturnsAsync(true);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _bankAccountService.DeleteById(accountId, default));

        // ASSERT
        Assert.Equal($"Account to delete with id: {accountId} should be closed before deletion!", exception.Message);
    }

    [Fact]
    public async Task DeleteById_SuccessPath_DeleteByIdInBankAccountRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var accountId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.IsOpened(accountId, default))
            .ReturnsAsync(false);

        // ACT
        await _bankAccountService.DeleteById(accountId, default);

        // ASSERT
        _mockBankAccountRepository
            .Verify(repository => repository.DeleteById(accountId, default), Times.Once);
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once);
    }

    private static IEnumerable<object[]> GetDataForGetAllTests()
    {
        var data = new List<object[]>
        {
            new object[]
            {
                new List<BankAccount>
                {
                    Capacity = 0
                }
            },
            new object[] { new List<BankAccount> { new(), new() } }
        };

        return data;
    }

    private static IEnumerable<object[]> GetDataForCalculateCommissionTests()
    {
        var sameGuid = Guid.NewGuid();

        var data = new List<object[]>
        {
            new object[]
            {
                100, new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid() },
                new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }, 2
            },
            new object[]
            {
                100, new BankAccount { Id = Guid.NewGuid(), UserId = sameGuid },
                new BankAccount { Id = Guid.NewGuid(), UserId = sameGuid }, 0
            }
        };

        return data;
    }

    private static IEnumerable<object[]> GetDataForTransferMoneyTest()
    {
        var sameGuid = Guid.NewGuid();

        var data = new List<object[]>
        {
            new object[]
            {
                new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), AmountOfMoney = 100 },
                new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), AmountOfMoney = 30 }, 50.75
            },
            new object[]
            {
                new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), AmountOfMoney = 50 },
                new BankAccount { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), AmountOfMoney = 30 }, 50
            },
            new object[]
            {
                new BankAccount { Id = Guid.NewGuid(), UserId = sameGuid, AmountOfMoney = 100 },
                new BankAccount { Id = Guid.NewGuid(), UserId = sameGuid, AmountOfMoney = 30 }, 50.75
            }
        };

        return data;
    }
}