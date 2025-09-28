using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.DTOs
{
    public class BetItemDto
    {
        public Guid MarketId { get; set; }

        public decimal OddsAtPlacement { get; set; }

        public string BetType { get; set; } = null!;
    }
}
