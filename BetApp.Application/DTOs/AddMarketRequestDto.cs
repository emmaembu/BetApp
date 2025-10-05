using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.DTOs
{
    public class AddMarketRequestDto
    {
        public Guid MatchId { get; set; }

        public string Type { get; set; } = null!;

        public decimal Odds { get; set; }

        public bool IsTopOffer { get; set; }

        public bool IsActive { get; set; }
    }
}
