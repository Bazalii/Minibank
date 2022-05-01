using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task Add(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            await _context.BankAccounts.AddAsync(new BankAccountDbModel
            {
                Id = bankAccount.Id,
                UserId = bankAccount.UserId,
                AmountOfMoney = bankAccount.AmountOfMoney,
                CurrencyCode = bankAccount.CurrencyCode,
                IsOpened = bankAccount.IsOpened,
                OpenDate = bankAccount.OpenDate,
                CloseDate = bankAccount.CloseDate
            }, cancellationToken);
        }

        public async Task<BankAccount> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dbModel = await _context.BankAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

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

        public async Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.BankAccounts.Select(account => new BankAccount
            {
                Id = account.Id,
                UserId = account.UserId,
                AmountOfMoney = account.AmountOfMoney,
                CurrencyCode = account.CurrencyCode,
                IsOpened = account.IsOpened,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate
            }).ToListAsync(cancellationToken);
        }

        public async Task Update(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            var dbModel =
                await _context.BankAccounts.FirstOrDefaultAsync(account => account.Id == bankAccount.Id,
                    cancellationToken);

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

        public async Task UpdateAccountMoney(Guid id, double amountOfMoney, CancellationToken cancellationToken)
        {
            var dbModel =
                await _context.BankAccounts.FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            dbModel.AmountOfMoney = amountOfMoney;
        }

        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var dbModel =
                await _context.BankAccounts.FirstOrDefaultAsync(currentAccount => currentAccount.Id == id,
                    cancellationToken);

            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {id} is not found!");
            }

            _context.BankAccounts.Remove(dbModel);
        }

        public Task<bool> ExistsForUser(Guid userId, CancellationToken cancellationToken)
        {
            return _context.BankAccounts.AnyAsync(account => account.UserId == userId, cancellationToken);
        }
        
        public async Task<bool> IsOpened(Guid accountId, CancellationToken cancellationToken)
        {
            var dbModel =
                await _context.BankAccounts.FirstOrDefaultAsync(account => account.Id == accountId, cancellationToken);

            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Account with id: {accountId} is not found!");
            }

            return dbModel.IsOpened;
        }
    }
}