using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.DbEntities;
using BetApp.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BetApp.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task EnsureSeedDataAsync(AppDbContext db)
        {

            await db.Database.MigrateAsync();

            // Walle
            if (!await db.Wallets.AnyAsync())
            {
                
                var wallet = new Wallet(Guid.NewGuid()); 
                wallet.Deposit(1000M, "Initial Deposit");
                wallet.Deposit(500M, "Second Deposit");
                var dbWallet = wallet.ToDbEntity();
                db.Wallets.Add(dbWallet);

                await db.SaveChangesAsync();
           

                //Transaction
                if(!await db.Transactions.AnyAsync())
                {
                    var dbTransactions = wallet.Transactions.Select(t => 
                    {
                        var dbT = t.ToDbEntity();
                        dbT.WalletId = dbWallet.Id;
                        return dbT;
                    }).ToList();
                    db.Transactions.AddRange(dbTransactions);

                    await db.SaveChangesAsync();
                }
            }

            // Matches & Markets
            if (!await db.Matches.AnyAsync())
            {
                var match1Id = Guid.NewGuid();
                var match2Id = Guid.NewGuid();

                var match1 = new MatchEntity
                {
                    Id = match1Id,
                    HomeTeam = "Team A",
                    AwayTeam = "Team B",
                    StartTime = DateTime.UtcNow.AddDays(1)
                };

                var match2 = new MatchEntity
                {
                    Id = match2Id,
                    HomeTeam = "Team C",
                    AwayTeam = "Team D",
                    StartTime = DateTime.UtcNow.AddDays(2)
                };

                db.Matches.AddRange(match1, match2);
                

                db.Markets.AddRange(
                    new MarketEntity
                    {
                        Id = Guid.NewGuid(),
                        MatchId = match1Id,
                        BetType = (int)BetType.Home, 
                        Odds = 1.80m,
                        IsTopOffer = false,
                        IsActive = true
                    },
                    new MarketEntity
                    {
                        Id = Guid.NewGuid(),
                        MatchId = match1Id,
                        BetType = (int)BetType.Away,
                        Odds = 2.10m,
                        IsTopOffer = true,
                        IsActive = true,
                        BoostedOdds=2.50M,
                        MaxStake= 10M
                    },
                    new MarketEntity
                    {
                        Id = Guid.NewGuid(),
                        MatchId = match2Id,
                        BetType = (int)BetType.Draw,
                        Odds = null,
                        IsTopOffer = false,
                        IsActive = false
                    }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}