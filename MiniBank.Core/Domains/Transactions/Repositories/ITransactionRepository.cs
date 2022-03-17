using System;

namespace MiniBank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);

        public Transaction GetTransactionById(Guid id);

        public void Update(Transaction transaction);

        public void DeleteTransactionById(Guid id);
    }
}