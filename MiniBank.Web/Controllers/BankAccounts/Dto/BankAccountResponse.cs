using System;
using MiniBank.Core.Enums;

namespace MiniBank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountResponse
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