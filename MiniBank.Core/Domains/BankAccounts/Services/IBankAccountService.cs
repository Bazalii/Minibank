using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task Add(BankAccountCreationModel model);

        Task<BankAccount> GetById(Guid id);

        Task<IEnumerable<BankAccount>> GetAll();

        Task Update(BankAccount bankAccount);

        Task UpdateMoneyOnAccount(Guid id, double amountOfMoney);

        Task CloseAccountById(Guid id);

        Task<double> CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId);

        Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId);
    }
}