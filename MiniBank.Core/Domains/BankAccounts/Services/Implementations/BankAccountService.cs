using System;
using System.Collections.Generic;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
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

        public void AddAccount(BankAccount bankAccount)
        {
            if (bankAccount.CurrencyCode != "RUB" && bankAccount.CurrencyCode != "USD" &&
                bankAccount.CurrencyCode != "EUR")
            {
                throw new ValidationException($"Invalid currency code: {bankAccount.CurrencyCode}");
            }

            _userRepository.CheckByIdIfUserExists(bankAccount.UserId);
            _bankAccountRepository.Add(bankAccount);
        }

        public BankAccount GetAccountById(Guid id)
        {
            return _bankAccountRepository.GetAccountById(id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public void UpdateAccount(BankAccount bankAccount)
        {
            _bankAccountRepository.Update(bankAccount);
        }

        public void UpdateMoneyOnAccount(Guid id, double amountOfMoney)
        {
            _bankAccountRepository.UpdateMoneyOnAccount(id, amountOfMoney);
        }

        public void CloseAccountById(Guid id)
        {
            var wantedAccount = _bankAccountRepository.GetAccountById(id);
            if (wantedAccount.AmountOfMoney != 0)
            {
                throw new ValidationException(
                    $"Amount of money on account with id: {id} that you want to close should be 0!");
            }

            _bankAccountRepository.Update(new BankAccount
            {
                Id = wantedAccount.Id,
                UserId = wantedAccount.UserId,
                AmountOfMoney = wantedAccount.AmountOfMoney,
                CurrencyCode = wantedAccount.CurrencyCode,
                Open = false,
                TimeOfOpening = wantedAccount.TimeOfOpening,
                TimeOfClosing = DateTime.Now
            });
        }

        public double CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            var withdrawalAccount = _bankAccountRepository.GetAccountById(withdrawalAccountId);
            var replenishmentAccount = _bankAccountRepository.GetAccountById(replenishmentAccountId);
            if (withdrawalAccount.UserId != replenishmentAccount.UserId)
            {
                return Math.Round(amount * 0.02, 2);
            }

            return 0;
        }

        public void TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            if (withdrawalAccountId == replenishmentAccountId)
            {
                throw new ValidationException("Money can be transferred only between different accounts!");
            }

            var withdrawalAccount = _bankAccountRepository.GetAccountById(withdrawalAccountId);
            var replenishmentAccount = _bankAccountRepository.GetAccountById(replenishmentAccountId);
            _bankAccountRepository.UpdateMoneyOnAccount(withdrawalAccountId, withdrawalAccount.AmountOfMoney - amount);
            var finalAmount = amount;
            if (withdrawalAccount.CurrencyCode != replenishmentAccount.CurrencyCode)
            {
                finalAmount = _currencyConverter.ConvertCurrency(finalAmount, withdrawalAccount.CurrencyCode,
                    replenishmentAccount.CurrencyCode);
            }

            finalAmount -= CalculateCommission(finalAmount, withdrawalAccountId, replenishmentAccountId);
            _bankAccountRepository.UpdateMoneyOnAccount(replenishmentAccountId, replenishmentAccount.AmountOfMoney + finalAmount);
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