using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task Add(BankAccountCreationModel model, CancellationToken cancellationToken);

        Task<BankAccount> GetById(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken);

        Task Update(BankAccount bankAccount, CancellationToken cancellationToken);

        Task UpdateMoneyOnAccount(Guid id, double amountOfMoney, CancellationToken cancellationToken);

        Task CloseAccountById(Guid id, CancellationToken cancellationToken);

        Task<double> CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId,
            CancellationToken cancellationToken);

        Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId,
            CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);
    }
}