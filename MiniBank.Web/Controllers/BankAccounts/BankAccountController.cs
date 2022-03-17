using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Core.Domains.BankAccounts.Services;
using MiniBank.Web.Controllers.BankAccounts.Dto;

namespace MiniBank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Route("/account")]
    public class BankAccountController
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost]
        public void Create(BankAccountRequestOnCreation request)
        {
            _bankAccountService.AddAccount(new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                AmountOfMoney = request.AmountOfMoney,
                CurrencyCode = request.CurrencyCode,
                Open = true,
                TimeOfOpening = DateTime.Now
            });
        }

        [HttpGet("{id:guid}")]
        public BankAccountResponse Get(Guid id)
        {
            var wantedAccount = _bankAccountService.GetAccountById(id);
            return new BankAccountResponse
            {
                Id = wantedAccount.Id,
                UserId = wantedAccount.UserId,
                AmountOfMoney = wantedAccount.AmountOfMoney,
                CurrencyCode = wantedAccount.CurrencyCode,
                Open = wantedAccount.Open,
                TimeOfOpening = wantedAccount.TimeOfOpening,
                TimeOfClosing = wantedAccount.TimeOfClosing
            };
        }

        [HttpGet]
        public IEnumerable<BankAccountResponse> GetAll()
        {
            return _bankAccountService.GetAll().Select(account => new BankAccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                AmountOfMoney = account.AmountOfMoney,
                CurrencyCode = account.CurrencyCode,
                Open = account.Open,
                TimeOfOpening = account.TimeOfOpening,
                TimeOfClosing = account.TimeOfClosing
            });
        }

        [HttpPut("{id:guid}")]
        public void Update(Guid id, BankAccountRequestOnMoneyUpdate request)
        {
            _bankAccountService.UpdateMoneyOnAccount(id, request.AmountOfMoney);
        }

        [HttpPost]
        [Route("/closeAccount")]
        public void CloseAccount(Guid id)
        {
            _bankAccountService.CloseAccountById(id);
        }

        [HttpPost]
        [Route("/transferMoney")]
        public void TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            _bankAccountService.TransferMoney(amount, withdrawalAccountId, replenishmentAccountId);
        }
    }
}