using System;

namespace MiniBank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountRequestOnCreation
    {
        public Guid UserId { get; set; }

        public double AmountOfMoney { get; set; }

        public string CurrencyCode { get; set; }
    }
}