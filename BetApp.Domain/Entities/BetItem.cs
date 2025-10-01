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
    }
}
