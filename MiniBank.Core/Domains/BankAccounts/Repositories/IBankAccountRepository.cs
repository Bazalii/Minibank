using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task Add(BankAccount bankAccount, CancellationToken cancellationToken);

        Task<BankAccount> GetById(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken);

        Task Update(BankAccount bankAccount, CancellationToken cancellationToken);

        Task UpdateAccountMoney(Guid id, double amountOfMoney, CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);

        Task<bool> ExistsForUser(Guid accountId, CancellationToken cancellationToken);
    }
}