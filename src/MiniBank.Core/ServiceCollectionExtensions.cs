﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MiniBank.Core.Domains.BankAccounts.Services;
using MiniBank.Core.Domains.BankAccounts.Services.Implementations;
using MiniBank.Core.Domains.CurrencyConverting.Services;
using MiniBank.Core.Domains.CurrencyConverting.Services.Implementations;
using MiniBank.Core.Domains.Providers;
using MiniBank.Core.Domains.Providers.Implementations;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Core.Domains.Users.Services.Implementations;

namespace MiniBank.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddFluentValidation()
                .AddValidatorsFromAssembly(typeof(UserService).Assembly);
            return services;
        }
    }
}