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
                IsOpened = bankAccount.IsOpened,
                OpenDate = bankAccount.OpenDate,
                CloseDate = bankAccount.CloseDate
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
                IsOpened = dbModel.IsOpened,
                OpenDate = dbModel.OpenDate,
                CloseDate = dbModel.CloseDate
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
                IsOpened = account.IsOpened,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate
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
            dbModel.IsOpened = bankAccount.IsOpened;
            dbModel.OpenDate = bankAccount.OpenDate;
            dbModel.CloseDate = bankAccount.CloseDate;
        }

        public void UpdateAccountMoney(Guid id, double amountOfMoney)
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

            if (dbModel.IsOpened)
            {
                throw new ValidationException(
                    $"Account to delete with id: {id} should be closed before deletion!");
            }

            _accounts.Remove(dbModel);
        }

        public bool ExistsForUser(Guid userId)
        {
            return _accounts.FirstOrDefault(account => account.UserId == userId) != null;
        }
    }
}