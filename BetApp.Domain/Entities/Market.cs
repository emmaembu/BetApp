using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class Market
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Match? Match { get; set; }
        public BetType Type { get; set; }
        public decimal? Odds { get; set; } // null => not available
        public bool IsTopOffer { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
