using System;
using System.Collections.Generic;
using System.Linq;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.Transactions.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly List<TransactionDbModel> _transactions = new();

        public void Add(Transaction transaction)
        {
            _transactions.Add(new TransactionDbModel
            {
                Id = transaction.Id,
                AmountOfMoney = transaction.AmountOfMoney,
                WithdrawalAccount = transaction.WithdrawalAccount,
                ReplenishmentAccount = transaction.ReplenishmentAccount
            });
        }

        public Transaction GetById(Guid id)
        {
            var dbModel = _transactions.FirstOrDefault(transaction => transaction.Id == id);
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
                _transactions.FirstOrDefault(currentTransaction => currentTransaction.Id == transaction.Id);
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
            var dbModel = _transactions.FirstOrDefault(transaction => transaction.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            _transactions.Remove(dbModel);
        }
    }
}