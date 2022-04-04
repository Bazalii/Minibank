using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Task Create(BankAccountCreationRequest model)
        {
            return _bankAccountService.Add(new BankAccountCreationModel
            {
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<BankAccountResponse> Get(Guid id)
        {
            var model = await _bankAccountService.GetById(id);
            return new BankAccountResponse
            {
                Id = model.Id,
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                IsOpened = model.IsOpened,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate
            };
        }

        [HttpGet]
        public async Task<IEnumerable<BankAccountResponse>> GetAll()
        {
            var bankAccounts = await _bankAccountService.GetAll();
            return bankAccounts.Select(account => new BankAccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                AmountOfMoney = account.AmountOfMoney,
                CurrencyCode = account.CurrencyCode,
                IsOpened = account.IsOpened,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate
            });
        }

        [HttpPut("{id:guid}")]
        public Task Update(Guid id, BankAccountMoneyUpdateRequest model)
        {
            return _bankAccountService.UpdateMoneyOnAccount(id, model.AmountOfMoney);
        }

        [HttpPost]
        [Route("/close")]
        public Task CloseAccount(Guid id)
        {
            return _bankAccountService.CloseAccountById(id);
        }

        [HttpPost]
        [Route("/transferMoney")]
        public Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            return _bankAccountService.TransferMoney(amount, withdrawalAccountId, replenishmentAccountId);
        }
    }
}