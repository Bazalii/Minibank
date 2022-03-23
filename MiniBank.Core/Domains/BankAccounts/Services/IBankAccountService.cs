using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        void Add(BankAccountCreationModel model);

        BankAccount GetById(Guid id);

        IEnumerable<BankAccount> GetAll();

        void Update(BankAccount bankAccount);

        void UpdateMoneyOnAccount(Guid id, double amountOfMoney);

        void CloseAccountById(Guid id);

        double CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId);

        void TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId);
    }
}