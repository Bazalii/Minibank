using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using MiniBank.Web.HostedServices;

namespace MiniBank.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddHostedService<MigrationHostedService>();
            services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            return services;
        }
    }
}