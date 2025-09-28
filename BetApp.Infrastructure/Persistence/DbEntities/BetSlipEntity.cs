using BetApp.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class BetSlipEntity
    {
        public Guid Id { get; set; }
        public Guid? WalletId { get; set; }
        public WalletEntity Wallet { get; set; } = null!;
        public decimal Stake { get; set; }
        public decimal TotalOdds { get; set; }
        public decimal FeePercent { get; set; } 
        public decimal PotentialPayout { get; set; }
        public DateTime PlacedAt { get; set; }
        public ICollection<BetItemEntity> BetItems { get; set; } = null!;
        public bool ContainsTopOffer => BetItems.Any(i=> i.Market.IsTopOffer);
    }
}
