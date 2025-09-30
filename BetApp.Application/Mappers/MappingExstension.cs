using BetApp.Application.DTOs;
using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Mappers
{
    public static class MappingExstension
    {
        // Wallet
        public static WalletDto ToDto(this Wallet wallet)
        {
            if (wallet == null) return null;
            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };
        }

        public static Wallet ToDomain(this WalletDto dto)
        {
            if (dto == null) return null;
            return new Wallet
            {
                Id = dto.Id,
                Balance = dto.Balance
            };
        }

        // BetSlip
        public static BetSlipRequestDto ToDto(this BetSlip betSlip)
        {
            if (betSlip == null) return null;
            return new BetSlipRequestDto
            {
                WalletId = betSlip.WalletId,
                TotalStake = betSlip.TotalStake,
                Items = betSlip.Items?.Select(i => i.ToDto()).ToList() ?? new List<BetItemDto>()
            };
        }

        public static BetSlip ToDomain(this BetSlipRequestDto dto, Guid betSlipId)
        {
            if (dto == null) return null;
            return new BetSlip
            {
                Id = betSlipId,
                WalletId = dto.WalletId,
                Stake = dto.TotalStake,
                Items = dto.Items?.Select(i => i.ToDomain()).ToList() ?? new List<BetItem>()
            };
        }

        // BetItem
        public static BetItemDto ToDto(this BetItem item)
        {
            if (item == null) return null;
            return new BetItemDto
            {
                MarketId = item.MarketId,
                OddsAtPlacement = item.OddsAtPlacement,
                BetType = item.Type.ToString()
            };
        }

        public static BetItem ToDomain(this BetItemDto dto)
        {
            if (dto == null) return null;
            return new BetItem
            {
                MarketId = dto.MarketId,
                OddsAtPlacement = dto.OddsAtPlacement,
                Type = Enum.Parse<BetType>(dto.BetType)
            };
        }


        // Mat
        public static MatchDto  ToDto(this Match item)
        {
            if (item == null) return null;
            return new MatchDto
            {
                Id = item.Id,
                Home = item.HomeTeam,
                Away = item.AwayTeam,
                StartTime = item.StartTime
            };
        }

        public static Match ToDomain(this MatchDto dto)
        {
            if (dto == null) return null;
            return new Match
            {
                Id = dto.Id,
                HomeTeam = dto.Home,
                AwayTeam = dto.Away,
                StartTime = dto.StartTime
            };
        }

        // Market
        public static MarketDto ToDto(this Market item)
        {
            if (item == null) return null;
            return new MarketDto
            {
                Id = item.Id,
                MatchId = item.MatchId,
                Type = item.Type.ToString(),//provjeri 
                Odds = (decimal)item.Odds, //provjeri ove nullable i to
                IsTopOffer = item.IsTopOffer,
                IsActive = item.IsActive
            };
        }

        public static MarketDto ToDomain(this Market dto)
        {
            if (dto == null) return null;
            return new MarketDto
            {
                Id = dto.Id,
                MatchId = dto.MatchId,
                Type = dto.Type.ToString(),
                Odds = (decimal)dto.Odds, //provjeri ove nullable i to
                IsTopOffer = dto.IsTopOffer,
                IsActive = dto.IsActive
            };
        }
    }
}