using System;

namespace MiniBank.Core.Domains.Transactions
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public double AmountOfMoney { get; set; }

        public Guid WithdrawalAccount { get; set; }

        public Guid ReplenishmentAccount { get; set; }
    }
}