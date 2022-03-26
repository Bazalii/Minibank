using System;
using System.Collections.Generic;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Enums;
using MiniBank.Core.Exceptions;

namespace MiniBank.Core.Domains.BankAccounts.Services.Implementations
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        private readonly IUserRepository _userRepository;

        private readonly ITransactionRepository _transactionRepository;

        private readonly ICurrencyConverter _currencyConverter;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository,
            ICurrencyConverter currencyConverter, ITransactionRepository transactionRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
        }

        public void Add(BankAccountCreationModel model)
        {
            if (!_userRepository.Exists(model.UserId))
            {
                throw new ValidationException($"User with id: {model.UserId} is not found!");
            }

            _bankAccountRepository.Add(new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                IsOpened = true,
                OpenDate = DateTime.Now
            });
        }

        public BankAccount GetById(Guid id)
        {
            return _bankAccountRepository.GetById(id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public void Update(BankAccount bankAccount)
        {
            _bankAccountRepository.Update(bankAccount);
        }

        public void UpdateMoneyOnAccount(Guid id, double amountOfMoney)
        {
            _bankAccountRepository.UpdateAccountMoney(id, amountOfMoney);
        }

        public void CloseAccountById(Guid id)
        {
            var model = _bankAccountRepository.GetById(id);
            if (model.AmountOfMoney != 0)
            {
                throw new ValidationException(
                    $"Amount of money on account with id: {id} that you want to close should be 0!");
            }

            model.IsOpened = false;
            model.CloseDate = DateTime.Now;
            
            _bankAccountRepository.Update(model);
        }

        public double CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            var withdrawalAccount = _bankAccountRepository.GetById(withdrawalAccountId);
            var replenishmentAccount = _bankAccountRepository.GetById(replenishmentAccountId);
            if (withdrawalAccount.UserId == replenishmentAccount.UserId) return 0;

            var result = Math.Round(amount * 0.02, 2);

            return result;
        }

        public void TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            if (withdrawalAccountId == replenishmentAccountId)
            {
                throw new ValidationException("Money can be transferred only between different accounts!");
            }

            var withdrawalAccount = _bankAccountRepository.GetById(withdrawalAccountId);
            var replenishmentAccount = _bankAccountRepository.GetById(replenishmentAccountId);

            _bankAccountRepository.UpdateAccountMoney(withdrawalAccountId, withdrawalAccount.AmountOfMoney - amount);

            var finalAmount = amount;
            if (withdrawalAccount.CurrencyCode != replenishmentAccount.CurrencyCode)
            {
                finalAmount = _currencyConverter.ConvertCurrency(finalAmount, withdrawalAccount.CurrencyCode,
                    replenishmentAccount.CurrencyCode);
            }

            finalAmount -= CalculateCommission(finalAmount, withdrawalAccountId, replenishmentAccountId);

            _bankAccountRepository.UpdateAccountMoney(replenishmentAccountId,
                replenishmentAccount.AmountOfMoney + finalAmount);

            _transactionRepository.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                WithdrawalAccount = withdrawalAccountId,
                ReplenishmentAccount = replenishmentAccountId,
                AmountOfMoney = finalAmount
            });
        }
    }
}