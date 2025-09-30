using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class BetSlip
    {
        public Guid Id { get; set; } 
        public Guid WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public decimal Stake { get; set; }
        public decimal TotalOdds { get; set; }
        public decimal FeePercent { get; set; } = 0.05m; // 5%
        public decimal PotentialPayout { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public ICollection<BetItem> Items { get; set; } = new List<BetItem>();
        public bool ContainsTopOffer => Items.Any(i => i.Market!.IsTopOffer);
        public decimal TotalStake => Items.Sum(i => i.Stake);
    }
}
