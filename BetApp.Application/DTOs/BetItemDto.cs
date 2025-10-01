using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.DTOs
{
    public class BetItemDto
    {
        public Guid MatchId { get; set; }

        public Guid MarketId { get; set; }

        public decimal OddsAtPlacement { get; set; }

        public BetType BetType { get; set; } 
    }
}
