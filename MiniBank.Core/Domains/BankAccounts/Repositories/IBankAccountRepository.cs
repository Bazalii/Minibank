using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        void Add(BankAccount bankAccount);

        BankAccount GetAccountById(Guid id);

        IEnumerable<BankAccount> GetAll();

        void Update(BankAccount bankAccount);

        void UpdateMoneyOnAccount(Guid id, double amountOfMoney);

        void DeleteAccountById(Guid id);

        bool CheckIfUserHasConnectedAccounts(Guid accountId);
    }
}