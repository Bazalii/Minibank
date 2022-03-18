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
            var dbModel = _accounts.FirstOrDefault(account => account.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            return new BankAccount
            {
                Id = dbModel.Id,
                UserId = dbModel.UserId,
                AmountOfMoney = dbModel.AmountOfMoney,
                CurrencyCode = dbModel.CurrencyCode,
                Open = dbModel.Open,
                TimeOfOpening = dbModel.TimeOfOpening,
                TimeOfClosing = dbModel.TimeOfClosing
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
            var dbModel = _accounts.FirstOrDefault(account => account.Id == bankAccount.Id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {bankAccount.Id} is not found!");
            }

            dbModel.Id = bankAccount.Id;
            dbModel.UserId = bankAccount.UserId;
            dbModel.AmountOfMoney = bankAccount.AmountOfMoney;
            dbModel.CurrencyCode = bankAccount.CurrencyCode;
            dbModel.Open = bankAccount.Open;
            dbModel.TimeOfOpening = bankAccount.TimeOfOpening;
            dbModel.TimeOfClosing = bankAccount.TimeOfClosing;
        }

        public void UpdateMoneyOnAccount(Guid id, double amountOfMoney)
        {
            var dbModel = _accounts.FirstOrDefault(account => account.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            dbModel.AmountOfMoney = amountOfMoney;
        }

        public void DeleteAccountById(Guid id)
        {
            var dbModel = _accounts.FirstOrDefault(currentAccount => currentAccount.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            if (dbModel.Open)
            {
                throw new ValidationException(
                    $"Account to delete with id: {id} should be closed before deletion!");
            }

            _accounts.Remove(dbModel);
        }

        public bool CheckIfUserHasConnectedAccounts(Guid userId)
        {
            return _accounts.FirstOrDefault(account => account.UserId == userId) != null;
        }
    }
}