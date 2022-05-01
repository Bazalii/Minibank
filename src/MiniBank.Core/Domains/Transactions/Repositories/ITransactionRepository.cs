using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction, CancellationToken cancellationToken);

        Task<Transaction> GetById(Guid id, CancellationToken cancellationToken);

        Task Update(Transaction transaction, CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);
    }
}