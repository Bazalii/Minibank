using System;
using System.Linq;
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

        public void Add(Transaction transaction)
        {
            _context.Transactions.Add(new TransactionDbModel
            {
                Id = transaction.Id,
                AmountOfMoney = transaction.AmountOfMoney,
                WithdrawalAccount = transaction.WithdrawalAccount,
                ReplenishmentAccount = transaction.ReplenishmentAccount
            });
        }

        public Transaction GetById(Guid id)
        {
            var dbModel = _context.Transactions
                .AsNoTracking()
                .FirstOrDefault(transaction => transaction.Id == id);
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

        public void Update(Transaction transaction)
        {
            var dbModel =
                _context.Transactions.FirstOrDefault(currentTransaction => currentTransaction.Id == transaction.Id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {transaction.Id} is not found!");
            }

            dbModel.Id = transaction.Id;
            dbModel.AmountOfMoney = transaction.AmountOfMoney;
            dbModel.WithdrawalAccount = transaction.WithdrawalAccount;
            dbModel.ReplenishmentAccount = transaction.ReplenishmentAccount;
        }

        public void DeleteById(Guid id)
        {
            var dbModel = _context.Transactions.FirstOrDefault(transaction => transaction.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            _context.Transactions.Remove(dbModel);
        }
    }
}