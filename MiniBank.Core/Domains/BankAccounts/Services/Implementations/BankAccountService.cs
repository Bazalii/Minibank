using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Enums;
using MiniBank.Core.Exceptions;

namespace MiniBank.Core.Domains.BankAccounts.Services.Implementations
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        private readonly IUserRepository _userRepository;

        private readonly ITransactionRepository _transactionRepository;

        private readonly ICurrencyConverter _currencyConverter;
        
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository,
            ICurrencyConverter currencyConverter, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(BankAccountCreationModel model)
        {
            var check = await _userRepository.Exists(model.UserId);
            
            if (!check)
            {
                throw new ValidationException($"User with id: {model.UserId} is not found!");
            }

            await _bankAccountRepository.Add(new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                AmountOfMoney = model.AmountOfMoney,
                CurrencyCode = model.CurrencyCode,
                IsOpened = true,
                OpenDate = DateTime.Now
            });

            await _unitOfWork.SaveChanges();
        }

        public async Task<BankAccount> GetById(Guid id)
        {
            return await _bankAccountRepository.GetById(id);
        }

        public async Task<IEnumerable<BankAccount>> GetAll()
        {
            return await _bankAccountRepository.GetAll();
        }

        public async Task Update(BankAccount bankAccount)
        {
            await _bankAccountRepository.Update(bankAccount);
            await _unitOfWork.SaveChanges();
        }

        public async Task UpdateMoneyOnAccount(Guid id, double amountOfMoney)
        {
            await _bankAccountRepository.UpdateAccountMoney(id, amountOfMoney);
            await _unitOfWork.SaveChanges();
        }

        public async Task CloseAccountById(Guid id)
        {
            var model = await _bankAccountRepository.GetById(id);
            if (model.AmountOfMoney != 0)
            {
                throw new ValidationException(
                    $"Amount of money on account with id: {id} that you want to close should be 0!");
            }

            model.IsOpened = false;
            model.CloseDate = DateTime.Now;
            
            await _bankAccountRepository.Update(model);
            await _unitOfWork.SaveChanges();
        }

        public async Task<double> CalculateCommission(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            var withdrawalAccount = await _bankAccountRepository.GetById(withdrawalAccountId);
            var replenishmentAccount = await _bankAccountRepository.GetById(replenishmentAccountId);
            if (withdrawalAccount.UserId == replenishmentAccount.UserId) return 0;

            var result = Math.Round(amount * 0.02, 2);

            return result;
        }

        public async Task TransferMoney(double amount, Guid withdrawalAccountId, Guid replenishmentAccountId)
        {
            if (withdrawalAccountId == replenishmentAccountId)
            {
                throw new ValidationException("Money can be transferred only between different accounts!");
            }

            var withdrawalAccount = await _bankAccountRepository.GetById(withdrawalAccountId);
            var replenishmentAccount = await _bankAccountRepository.GetById(replenishmentAccountId);

            await _bankAccountRepository.UpdateAccountMoney(withdrawalAccountId, withdrawalAccount.AmountOfMoney - amount);

            var finalAmount = amount;
            if (withdrawalAccount.CurrencyCode != replenishmentAccount.CurrencyCode)
            {
                finalAmount = await _currencyConverter.ConvertCurrency(finalAmount, withdrawalAccount.CurrencyCode,
                    replenishmentAccount.CurrencyCode);
            }

            var commission = await CalculateCommission(finalAmount, withdrawalAccountId, replenishmentAccountId);
            finalAmount -= commission;

            await _bankAccountRepository.UpdateAccountMoney(replenishmentAccountId,
                replenishmentAccount.AmountOfMoney + finalAmount);

            await _transactionRepository.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                WithdrawalAccount = withdrawalAccountId,
                ReplenishmentAccount = replenishmentAccountId,
                AmountOfMoney = finalAmount
            });
            
            await _unitOfWork.SaveChanges();
        }
    }
}