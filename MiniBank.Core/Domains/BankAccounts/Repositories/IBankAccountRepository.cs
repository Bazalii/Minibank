using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task Add(BankAccount bankAccount);

        Task<BankAccount> GetById(Guid id);

        Task<IEnumerable<BankAccount>> GetAll();

        Task Update(BankAccount bankAccount);

        Task UpdateAccountMoney(Guid id, double amountOfMoney);

        Task DeleteById(Guid id);

        Task<bool> ExistsForUser(Guid accountId);
    }
}