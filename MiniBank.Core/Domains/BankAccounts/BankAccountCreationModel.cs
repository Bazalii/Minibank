using System;
using MiniBank.Core.Enums;

namespace MiniBank.Core.Domains.BankAccounts
{
    public class BankAccountCreationModel
    {
        public Guid UserId { get; set; }

        public double AmountOfMoney { get; set; }

        public Currencies CurrencyCode { get; set; }
    }
}