using System;

namespace MiniBank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);

        Transaction GetById(Guid id);

        void Update(Transaction transaction);

        void DeleteById(Guid id);
    }
}