using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public Task Create(BankAccountCreationRequest model, CancellationToken cancellationToken)
        {
            return _bankAccountService.Add(new BankAccountCreationModel
            {
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode
            }, cancellationToken);
        }

        [HttpGet("{id:guid}")]
        public async Task<BankAccountResponse> Get(Guid id, CancellationToken cancellationToken)
        {
            var model = await _bankAccountService.GetById(id, cancellationToken);
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
        public async Task<IEnumerable<BankAccountResponse>> GetAll(CancellationToken cancellationToken)
        {
            var bankAccounts = await _bankAccountService.GetAll(cancellationToken);
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
        public Task Update(Guid id, BankAccountMoneyUpdateRequest model, CancellationToken cancellationToken)
        {
            return _bankAccountService.UpdateMoneyOnAccount(id, model.AmountOfMoney, cancellationToken);
        }

        [HttpPost]
        [Route("/close")]
        public Task CloseAccount(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountService.CloseAccountById(id, cancellationToken);
        }

        [HttpPost]
        [Route("/transferMoney")]
        public Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId,
            CancellationToken cancellationToken)
        {
            return _bankAccountService.TransferMoney(amount, withdrawalAccountId, replenishmentAccountId,
                cancellationToken);
        }

        [HttpDelete]
        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return _bankAccountService.DeleteById(id, cancellationToken);
        }
    }
}