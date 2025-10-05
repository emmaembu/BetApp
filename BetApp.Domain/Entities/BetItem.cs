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
        public Guid MarketId { get; set; }
        public Guid MatchId { get; set; }
        public decimal OddsAtPlacement { get; set; }
        public BetType Type { get; set; }       
        public decimal Stake { get; set; }

        public bool WasTopOffer { get; private set; } = false;

        private BetItem() { }

        public BetItem( Guid marketId, Guid matchId, decimal oddsAtPlacement, BetType type, decimal stake)
        {
            Id = Guid.NewGuid();
            MarketId = marketId;
            MatchId = matchId;
            OddsAtPlacement = oddsAtPlacement;
            Type = type;
            Stake = stake;
        }

        public void MarkAsTopOffer()
        {
            WasTopOffer = true;
        }

    }
}
