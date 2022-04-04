using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Exceptions;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MiniBankContext _context;

        public BankAccountRepository(MiniBankContext context)
        {
            _context = context;
        }

        public void Add(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(new BankAccountDbModel
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

        public BankAccount GetById(Guid id)
        {
            var dbModel = _context.BankAccounts
                .AsNoTracking()
                .FirstOrDefault(account => account.Id == id);
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
            return _context.BankAccounts.Select(account => new BankAccount
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
            var dbModel = _context.BankAccounts.FirstOrDefault(account => account.Id == bankAccount.Id);
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
            var dbModel = _context.BankAccounts.FirstOrDefault(account => account.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            dbModel.AmountOfMoney = amountOfMoney;
        }

        public void DeleteById(Guid id)
        {
            var dbModel = _context.BankAccounts.FirstOrDefault(currentAccount => currentAccount.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            if (dbModel.IsOpened)
            {
                throw new ValidationException(
                    $"Account to delete with id: {id} should be closed before deletion!");
            }

            _context.BankAccounts.Remove(dbModel);
        }

        public bool ExistsForUser(Guid userId)
        {
            return _context.BankAccounts.FirstOrDefault(account => account.UserId == userId) != null;
        }
    }
}