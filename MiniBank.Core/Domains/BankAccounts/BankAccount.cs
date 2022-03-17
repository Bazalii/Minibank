using System;

namespace MiniBank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public double AmountOfMoney { get; set; }

        public string CurrencyCode { get; set; }

        public bool Open { get; set; }

        public DateTime TimeOfOpening { get; set; }

        public DateTime TimeOfClosing { get; set; }
    }
}