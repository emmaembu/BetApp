using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class BetItemEntity
    {
        public Guid Id { get; set; }
        public Guid BetSlipId { get; set; }
        public BetSlipEntity BetSlip { get; set; }
        public Guid MarketId { get; set; }
        public MarketEntity Market { get; set; } = null!;
        public decimal OddsAtPlacement { get; set; }
        public int BetType { get; set; }
    }
}
