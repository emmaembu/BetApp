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
        //public decimal Stake { get; set; }
        //public decimal TotalOdds { get; set; }
        public decimal FeePercent { get; set; } = 0.05m; // 5%
        public decimal FeeAmount => TotalStake*FeePercent;
        public decimal NetStake => TotalStake - FeePercent;
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public List<BetItem> Items { get; set; } = new List<BetItem>();
        public decimal TotalStake => Items.Sum(i => i.Stake);
    }
}
