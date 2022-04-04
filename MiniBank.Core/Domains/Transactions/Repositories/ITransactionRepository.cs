using System;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction);

        Task<Transaction> GetById(Guid id);

        Task Update(Transaction transaction);

        Task DeleteById(Guid id);
    }
}