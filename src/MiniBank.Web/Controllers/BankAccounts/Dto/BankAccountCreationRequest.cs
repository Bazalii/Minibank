using System;
using MiniBank.Core.Enums;

namespace MiniBank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountCreationRequest
    {
        public Guid UserId { get; set; }

        public double AmountOfMoney { get; set; }

        public Currencies CurrencyCode { get; set; }
    }
}