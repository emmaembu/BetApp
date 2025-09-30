using BetApp.Application.Interfaces;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IBetRepository, BetRepository>();
            services.AddScoped<IMarketRepository, MarketRepository>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();

            return services;
        }
    }
}
