using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using BetApp.Infrastructure.Persistence.Configurations;
using BetApp.Infrastructure.Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BetApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MatchEntity> Matches { get; set; } = null!;
        public DbSet<MarketEntity> Markets { get; set; } = null!;
        public DbSet<WalletEntity> Wallets { get; set; } = null!;
        public DbSet<TransactionEntity> Transactions { get; set; } = null!;
        public DbSet<BetSlipEntity> BetSlips { get; set; } = null!;
        public DbSet<BetItemEntity> BetItems { get; set; } = null!;

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Apply all IEntityTypeConfiguration<T> from this assembly (Configurations folder)
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
   
                base.OnModelCreating(modelBuilder);
            }
        }
    }