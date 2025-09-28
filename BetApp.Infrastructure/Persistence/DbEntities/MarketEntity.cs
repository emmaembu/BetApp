using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class MarketEntity
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; } = null!;
        public int BetType { get; set; }
        public decimal? Odds { get; set; } // null => not available
        public bool IsTopOffer { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? BoostedOdds { get; set; }
        public decimal? MaxStake { get; set; }
    }
}
