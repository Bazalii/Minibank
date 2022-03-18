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
        public void Create(BankAccountCreationRequest model)
        {
            _bankAccountService.AddAccount(new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                Open = true,
                TimeOfOpening = DateTime.Now
            });
        }

        [HttpGet("{id:guid}")]
        public BankAccountResponse Get(Guid id)
        {
            var model = _bankAccountService.GetAccountById(id);
            return new BankAccountResponse
            {
                Id = model.Id,
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                Open = model.Open,
                TimeOfOpening = model.TimeOfOpening,
                TimeOfClosing = model.TimeOfClosing
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
        public void Update(Guid id, BankAccountMoneyUpdateRequest model)
        {
            _bankAccountService.UpdateMoneyOnAccount(id, model.AmountOfMoney);
        }

        [HttpPost]
        [Route("/close")]
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