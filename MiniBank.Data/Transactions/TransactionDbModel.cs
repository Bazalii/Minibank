using System;

namespace MiniBank.Data.Transactions
{
    public class TransactionDbModel
    {
        public Guid Id { get; set; }

        public double AmountOfMoney { get; set; }

        public Guid WithdrawalAccount { get; set; }

        public Guid ReplenishmentAccount { get; set; }
    }
}