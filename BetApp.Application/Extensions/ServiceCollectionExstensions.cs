using BetApp.Application.Interfaces;
using BetApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace BetApp.Application.Extensions
{
    public static class ServiceCollectionExstensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IMarketService, MarketService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IMatchService, MatchService>();

            return services;
        }

    }
}
