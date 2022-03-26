using System;
using MiniBank.Core.Enums;

namespace MiniBank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public double AmountOfMoney { get; set; }

        public Currencies CurrencyCode { get; set; }

        public bool IsOpened { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }
    }
}