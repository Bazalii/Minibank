using System;
using System.Collections.Generic;
using System.Linq;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Exceptions;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly List<BankAccountDbModel> _accounts = new();

        public void Add(BankAccount bankAccount)
        {
            _accounts.Add(new BankAccountDbModel
            {
                Id = bankAccount.Id,
                UserId = bankAccount.UserId,
                AmountOfMoney = bankAccount.AmountOfMoney,
                CurrencyCode = bankAccount.CurrencyCode,
                Open = bankAccount.Open,
                TimeOfOpening = bankAccount.TimeOfOpening,
                TimeOfClosing = bankAccount.TimeOfClosing
            });
        }

        public BankAccount GetAccountById(Guid id)
        {
            var wantedAccount = _accounts.FirstOrDefault(account => account.Id == id);
            if (wantedAccount == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            return new BankAccount
            {
                Id = wantedAccount.Id,
                UserId = wantedAccount.UserId,
                AmountOfMoney = wantedAccount.AmountOfMoney,
                CurrencyCode = wantedAccount.CurrencyCode,
                Open = wantedAccount.Open,
                TimeOfOpening = wantedAccount.TimeOfOpening,
                TimeOfClosing = wantedAccount.TimeOfClosing
            };
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _accounts.Select(account => new BankAccount
            {
                Id = account.Id,
                UserId = account.UserId,
                AmountOfMoney = account.AmountOfMoney,
                CurrencyCode = account.CurrencyCode,
                Open = account.Open,
                TimeOfOpening = account.TimeOfOpening,
                TimeOfClosing = account.TimeOfClosing
            });
        }

        public void Update(BankAccount bankAccount)
        {
            var wantedAccount = _accounts.FirstOrDefault(account => account.Id == bankAccount.Id);
            if (wantedAccount == null)
            {
                throw new ObjectNotFoundException($"Account with id: {bankAccount.Id} is not found!");
            }

            wantedAccount.Id = bankAccount.Id;
            wantedAccount.UserId = bankAccount.UserId;
            wantedAccount.AmountOfMoney = bankAccount.AmountOfMoney;
            wantedAccount.CurrencyCode = bankAccount.CurrencyCode;
            wantedAccount.Open = bankAccount.Open;
            wantedAccount.TimeOfOpening = bankAccount.TimeOfOpening;
            wantedAccount.TimeOfClosing = bankAccount.TimeOfClosing;
        }

        public void UpdateMoneyOnAccount(Guid id, double amountOfMoney)
        {
            var wantedAccount = _accounts.FirstOrDefault(account => account.Id == id);
            if (wantedAccount == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }
            
            wantedAccount.AmountOfMoney = amountOfMoney;
        }

        public void DeleteAccountById(Guid id)
        {
            var wantedAccount = _accounts.FirstOrDefault(currentAccount => currentAccount.Id == id);
            if (wantedAccount == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            if (wantedAccount.Open)
            {
                throw new ValidationException(
                    $"Account to delete with id: {id} should be closed before deletion!");
            }

            _accounts.Remove(wantedAccount);
        }

        public bool CheckIfUserHasConnectedAccounts(Guid userId)
        {
            return _accounts.FirstOrDefault(account => account.UserId == userId) != null;
        }
    }
}