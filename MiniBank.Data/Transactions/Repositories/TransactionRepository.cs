using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.Transactions.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MiniBankContext _context;

        public TransactionRepository(MiniBankContext context)
        {
            _context = context;
        }

        public async Task Add(Transaction transaction)
        {
            await _context.Transactions.AddAsync(new TransactionDbModel
            {
                Id = transaction.Id,
                AmountOfMoney = transaction.AmountOfMoney,
                WithdrawalAccount = transaction.WithdrawalAccount,
                ReplenishmentAccount = transaction.ReplenishmentAccount
            });
        }

        public async Task<Transaction> GetById(Guid id)
        {
            var dbModel = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(transaction => transaction.Id == id);
            
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            return new Transaction
            {
                Id = dbModel.Id,
                AmountOfMoney = dbModel.AmountOfMoney,
                WithdrawalAccount = dbModel.WithdrawalAccount,
                ReplenishmentAccount = dbModel.ReplenishmentAccount
            };
        }

        public async Task Update(Transaction transaction)
        {
            var dbModel =
                await _context.Transactions.FirstOrDefaultAsync(currentTransaction =>
                    currentTransaction.Id == transaction.Id);
            
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {transaction.Id} is not found!");
            }

            dbModel.Id = transaction.Id;
            dbModel.AmountOfMoney = transaction.AmountOfMoney;
            dbModel.WithdrawalAccount = transaction.WithdrawalAccount;
            dbModel.ReplenishmentAccount = transaction.ReplenishmentAccount;
        }

        public async Task DeleteById(Guid id)
        {
            var dbModel = await _context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);
            
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            _context.Transactions.Remove(dbModel);
        }
    }
}