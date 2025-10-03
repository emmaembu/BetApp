using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class BetItem
    {
        public Guid Id { get; set; }
        public Guid BetSlipId { get; set; }
        public Guid MarketId { get; set; }
        public Guid MatchId { get; set; }
        public decimal OddsAtPlacement { get; set; }
        public BetType Type { get; set; }       
        public decimal Stake { get; set; }

        private BetItem() { }

        public BetItem(Guid betSlipId, Guid marketId, Guid matchId, decimal oddsAtPlacement, BetType type, decimal stake)
        {
            Id = Guid.NewGuid();
            BetSlipId = betSlipId;
            MarketId = marketId;
            MatchId = matchId;
            OddsAtPlacement = oddsAtPlacement;
            Type = type;
            stake = Stake;
        }
    }
}
