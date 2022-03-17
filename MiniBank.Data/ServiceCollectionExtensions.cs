using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.Transactions.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Data.BankAccounts.Repositories;
using MiniBank.Data.ConvertingServices.HttpClients.Implementations;
using MiniBank.Data.Transactions.Repositories;
using MiniBank.Data.Users.Repositories;

namespace MiniBank.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBankAccountRepository, BankAccountRepository>();
            services.AddSingleton<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddHttpClient<IExchangeRateProvider, ExchangeRateHttpProvider>(options =>
            {
                options.BaseAddress = new Uri(configuration["ExchangeRateHttpProviderUri"]);
            });

            return services;
        }
    }
}