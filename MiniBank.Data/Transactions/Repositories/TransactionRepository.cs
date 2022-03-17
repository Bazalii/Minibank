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

        public Transaction GetTransactionById(Guid id)
        {
            var wantedTransaction = _transactions.FirstOrDefault(transaction => transaction.Id == id);
            if (wantedTransaction == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            return new Transaction
            {
                Id = wantedTransaction.Id,
                AmountOfMoney = wantedTransaction.AmountOfMoney,
                WithdrawalAccount = wantedTransaction.WithdrawalAccount,
                ReplenishmentAccount = wantedTransaction.ReplenishmentAccount
            };
        }

        public void Update(Transaction transaction)
        {
            var wantedTransaction =
                _transactions.FirstOrDefault(currentTransaction => currentTransaction.Id == transaction.Id);
            if (wantedTransaction == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {transaction.Id} is not found!");
            }

            wantedTransaction.Id = transaction.Id;
            wantedTransaction.AmountOfMoney = transaction.AmountOfMoney;
            wantedTransaction.WithdrawalAccount = transaction.WithdrawalAccount;
            wantedTransaction.ReplenishmentAccount = transaction.ReplenishmentAccount;
        }

        public void DeleteTransactionById(Guid id)
        {
            var wantedTransaction = _transactions.FirstOrDefault(transaction => transaction.Id == id);
            if (wantedTransaction == null)
            {
                throw new ObjectNotFoundException($"Transaction with id: {id} is not found!");
            }

            _transactions.Remove(wantedTransaction);
        }
    }
}