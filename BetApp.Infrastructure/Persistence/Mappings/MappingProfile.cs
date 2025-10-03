using BetApp.Infrastructure.Persistence.DbEntities;
using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using System.Linq;
using BetApp.Domain.Events;
using Microsoft.IdentityModel.Tokens;

namespace BetApp.Infrastructure.Persistence.Mappings
{
    //public class MappingProfile : Profile
    //{
    //    public MappingProfile() 
    //    {
    //        CreateMap<MatchEntity, Match>()
    //            .ForMember(dest => dest.Markets, opt => opt.MapFrom(src => src.Markets));
    //        CreateMap<MarketEntity, Market>()
    //            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (BetType)src.BetType))
    //            .ForMember(dest => dest.Match, opt => opt.MapFrom(src => src.Match));
    //        CreateMap<WalletEntity, Wallet>()
    //            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));
    //        CreateMap<TransactionEntity, Transaction>();
    //        CreateMap<BetItemEntity, BetItem>()
    //            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (BetType) src.BetType))
    //            .ForMember(dest => dest.Market, opt => opt.MapFrom(src => src.Market))
    //            .ForMember(dest => dest.BetSlip, opt => opt.MapFrom(src => src.BetSlip));
    //        CreateMap<BetSlipEntity, BetSlip>()
    //            .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => src.Wallet))
    //            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BetItems));
    //    }
    //}
    public static class MappingProfile
    {
            // MatchEntity -> Match (Domain)
            public static Match ToDomain(this MatchEntity entity)
            {
                if (entity == null) return null!;

                return new Match
                {
                    Id = entity.Id,
                    HomeTeam = entity.HomeTeam,
                    AwayTeam = entity.AwayTeam,
                    StartTime = entity.StartTime,
                    Markets = entity.Markets?.Select(m => m.ToDomain()).ToList() ?? new List<Market>()
                };
            }

            // MarketEntity -> Market (Domain)
            public static Market ToDomain(this MarketEntity entity)
            {
                if (entity == null) return null!;

                return new Market
                {
                    Id = entity.Id,
                    MatchId = entity.MatchId,
                    Type = (BetType)entity.BetType,
                    Odds = entity.Odds,
                    IsTopOffer = entity.IsTopOffer,
                    IsActive = entity.IsActive
                };
            }

            // WalletEntity -> Wallet (Domain)
            public static Wallet ToDomain(this WalletEntity entity)
            {
                if (entity == null) return null!;

                var wallet = new Wallet(entity.Id){ };

                typeof(Wallet).GetProperty("Balance")!.SetValue(wallet, entity.Balance);
                typeof(Wallet).GetProperty("UpdatedAt")!.SetValue(wallet, entity.UpdatedAt);
                typeof(Wallet).GetProperty("RowVersion")!.SetValue(wallet, entity.RowVersion);
                foreach (var tEntity in entity.Transactions)
                {
                    var transaction = tEntity.ToDomain();
                    var transactionsField = typeof(Wallet).GetField("_transactions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (transactionsField == null)
                        throw new InvalidOperationException("_transactions field not found");

                    var listObj = transactionsField.GetValue(wallet);

                    if (listObj is List<Transaction> list)
                    {
                        list.Add(transaction);
                    }
                    else
                    {
                        throw new InvalidOperationException("_transactions field is not List<Transaction>");
                    }
                }
               return wallet;
            }

            // TransactionEntity -> Transaction (Domain)

            // BetSlipEntity -> BetSlip (Domain)
            public static Transaction ToDomain(this TransactionEntity entity)
            {
                if (entity == null) return null!;

                var transaction = (Transaction)Activator.CreateInstance(typeof(Transaction), true)!;

                typeof(Transaction).GetProperty("Id")!.SetValue(transaction, entity.Id);
                typeof(Transaction).GetProperty("WalletId")!.SetValue(transaction, entity.WalletId);
                typeof(Transaction).GetProperty("Amount")!.SetValue(transaction, entity.Amount);
                typeof(Transaction).GetProperty("BalanceBefore")!.SetValue(transaction, entity.BalanceBefore);
                typeof(Transaction).GetProperty("BalanceAfter")!.SetValue(transaction, entity.BalanceAfter);
                typeof(Transaction).GetProperty("TransactionType")!.SetValue(transaction, entity.TransactionType);
                typeof(Transaction).GetProperty("Description")!.SetValue(transaction, entity.Description);
                typeof(Transaction).GetProperty("CreatedAt")!.SetValue(transaction, entity.CreatedAt);
                return transaction;
            }
            private const decimal DefaultFeePercent = 0.05M;
            public static BetSlip ToDomain(this BetSlipEntity entity)
            {
                
                if (entity == null) return null!;

                var betItems = entity.BetItems?.Select(bi => bi.ToDomain()).ToList() ?? new List<BetItem>();
                var totalStake = betItems.Sum(bi => bi.Stake);
                var feePercent = DefaultFeePercent;
                var feeAmount = totalStake * feePercent;
                var netStake = totalStake - feeAmount;

                return new BetSlip
                (
                    (Guid)entity.WalletId!, 
                    betItems.AsReadOnly(),
                    entity.Payout
            );
            }

            // BetItemEntity -> BetItem (Domain)
            public static BetItem ToDomain(this BetItemEntity entity)
            {

                if (entity == null) return null!;

            return new BetItem
            (
                entity.BetSlipId,
                entity.MarketId,
                entity.MatchId,
                entity.OddsAtPlacement,
                (BetType)entity.BetType,
                entity.Stake
            );
        }

            // Domain -> DbEntity
            public static MatchEntity ToDbEntity(this Match domain)
            {
                if (domain == null) return null!;

                return new MatchEntity
                {
                    Id = domain.Id,
                    HomeTeam = domain.HomeTeam,
                    AwayTeam = domain.AwayTeam,
                    StartTime = domain.StartTime,
                    Markets = domain.Markets?.Cast<Market>().Select(m => m.ToDbEntity()).ToList() ?? new List<MarketEntity>()
                };
            }

            public static MarketEntity ToDbEntity(this Market domain)
            {
                if (domain == null) return null!;

                return new MarketEntity
                {
                    Id = domain.Id,
                    MatchId = domain.MatchId,
                    BetType = (int)domain.Type,
                    Odds = domain.Odds,
                    IsTopOffer = domain.IsTopOffer,
                    IsActive = domain.IsActive
                };
            }

            public static WalletEntity ToDbEntity(this Wallet domain)
            {
                if (domain == null) return null!;

            return new WalletEntity
            {
                Id = domain.Id,
                Balance = domain.Balance,
                UpdatedAt = domain.UpdatedAt
                // Transactions = domain.Transactions?.Cast<Transaction>().Select(t => t.ToDbEntity()).ToList() ?? new List<TransactionEntity>()
            };
            }

            public static TransactionEntity ToDbEntity(this Transaction domain)
            {
                if (domain == null) return null!;

                return new TransactionEntity 
                { 
                    Amount = domain.Amount,
                    BalanceBefore = domain.BalanceBefore,
                    BalanceAfter = domain.BalanceAfter,
                    CreatedAt = domain.CreatedAt,
                    Description = domain.Description,
                    TransactionType = (int)domain.TransactionType
                };
            }

            public static BetSlipEntity ToDbEntity(this BetSlip domain)
            {
                if (domain == null) return null!;

                return new BetSlipEntity
                {
                    Id = domain.Id,
                    WalletId = domain.WalletId,
                    BetItems = domain.Items?.Cast<BetItem>().Select(i => i.ToDbEntity()).ToList() ?? new List<BetItemEntity>()
                };
            }

            public static BetItemEntity ToDbEntity(this BetItem domain)
            {
                if (domain == null) return null!;

                return new BetItemEntity
                {
                    Id = domain.Id,
                    //BetSlipId = domain.BetSlipId,
                    MarketId = domain.MarketId,
                    OddsAtPlacement = domain.OddsAtPlacement,
                    BetType = (int)domain.Type
                };
            }

        public static OutboxMessageEntity ToDbEntity(this OutboxMessage domain)
        {
            return new OutboxMessageEntity
            {
                Id = domain.Id,
                AggregateId = domain.AggregateId,
                OccuredOn = domain.OccuredOn,
                ProcessedOn = domain.ProcessedOn,
                Type = domain.Type,
                Payload = domain.Payload,
                Error = domain.Error ?? ""
            };

        }

        public static OutboxMessage ToDomain(this OutboxMessageEntity entity)
        {
           return new OutboxMessage(entity.Type, entity.AggregateId, entity.Payload)
            {
                Id = entity.Id,
                OccuredOn = entity.OccuredOn,
                ProcessedOn = (DateTime)entity.ProcessedOn, // re-do
                Payload = entity.Payload
            };
        }
        }
    }
