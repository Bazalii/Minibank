using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using ValidationException = MiniBank.Core.Exceptions.ValidationException;

namespace MiniBank.Core.Domains.BankAccounts.Services.Implementations
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        private readonly ITransactionRepository _transactionRepository;

        private readonly ICurrencyConverter _currencyConverter;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<BankAccountCreationModel> _bankAccountValidator;

        public BankAccountService(IBankAccountRepository bankAccountRepository, ICurrencyConverter currencyConverter,
            ITransactionRepository transactionRepository, IUnitOfWork unitOfWork,
            IValidator<BankAccountCreationModel> bankAccountValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _bankAccountValidator = bankAccountValidator;
        }

        public async Task Add(BankAccountCreationModel model, CancellationToken cancellationToken)
        {
            await _bankAccountValidator.ValidateAndThrowAsync(model, cancellationToken);

            await _bankAccountRepository.Add(new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                IsOpened = true,
                OpenDate = DateTime.UtcNow
            }, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public Task<BankAccount> GetById(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetById(id, cancellationToken);
        }

        public Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken)
        {
            return _bankAccountRepository.GetAll(cancellationToken);
        }

        public async Task Update(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            await _bankAccountRepository.Update(bankAccount, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task UpdateMoneyOnAccount(Guid id, double amountOfMoney, CancellationToken cancellationToken)
        {
            await _bankAccountRepository.UpdateAccountMoney(id, amountOfMoney, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task CloseAccountById(Guid id, CancellationToken cancellationToken)
        {
            var model = await _bankAccountRepository.GetById(id, cancellationToken);

            if (model.AmountOfMoney != 0)
            {
                throw new ValidationException(
                    $"Amount of money on account with id: {id} that you want to close should be 0!");
            }

            model.IsOpened = false;
            model.CloseDate = DateTime.UtcNow;

            await _bankAccountRepository.Update(model, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task<double> CalculateCommission(double amount, Guid withdrawalAccountId,
            Guid replenishmentAccountId, CancellationToken cancellationToken)
        {
            var withdrawalAccount = await _bankAccountRepository.GetById(withdrawalAccountId, cancellationToken);
            var replenishmentAccount = await _bankAccountRepository.GetById(replenishmentAccountId, cancellationToken);

            if (withdrawalAccount.UserId == replenishmentAccount.UserId) return 0;

            var result = Math.Round(amount * 0.02, 2);

            return result;
        }

        public async Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId,
            CancellationToken cancellationToken)
        {
            if (withdrawalAccountId == replenishmentAccountId)
            {
                throw new ValidationException("Money can be transferred only between different accounts!");
            }

            var withdrawalAccount = await _bankAccountRepository.GetById(withdrawalAccountId, cancellationToken);
            var replenishmentAccount = await _bankAccountRepository.GetById(replenishmentAccountId, cancellationToken);

            if (withdrawalAccount.AmountOfMoney - amount < 0)
            {
                throw new ValidationException("Not enough money to transfer!");
            }
            
            await _bankAccountRepository.UpdateAccountMoney(withdrawalAccountId,
                withdrawalAccount.AmountOfMoney - amount, cancellationToken);

            var finalAmount = amount;
            if (withdrawalAccount.CurrencyCode != replenishmentAccount.CurrencyCode)
            {
                finalAmount = await _currencyConverter.ConvertCurrency(finalAmount, withdrawalAccount.CurrencyCode,
                    replenishmentAccount.CurrencyCode);
            }

            var commission = await CalculateCommission(finalAmount, withdrawalAccountId, replenishmentAccountId,
                cancellationToken);
            finalAmount -= commission;

            await _bankAccountRepository.UpdateAccountMoney(replenishmentAccountId,
                replenishmentAccount.AmountOfMoney + finalAmount, cancellationToken);

            await _transactionRepository.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                WithdrawalAccount = withdrawalAccountId,
                ReplenishmentAccount = replenishmentAccountId,
                AmountOfMoney = finalAmount
            });

            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var check = await _bankAccountRepository.IsOpened(id, cancellationToken);

            if (check)
            {
                throw new ValidationException(
                    $"Account to delete with id: {id} should be closed before deletion!");
            }
            
            await _bankAccountRepository.DeleteById(id, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }
    }
}