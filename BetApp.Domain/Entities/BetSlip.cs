using BetApp.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class BetSlip
    {
        public Guid Id { get; set; } 
        public Guid WalletId { get; set; }
        public decimal FeePercent { get; set; } = 0.05m; // 5%
        public decimal FeeAmount => TotalStake*FeePercent;
        public decimal NetStake => TotalStake - FeePercent;
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        //private readonly List<BetItem> _betItems= new();
        public IReadOnlyCollection<BetItem> Items { get; private set; }// => _betItems.AsReadOnly();
        public decimal TotalStake => Items.Sum(i => i.Stake);
        public decimal Payout { get; private set; }

        //private BetSlip() { }

        public BetSlip(Guid walletId, IReadOnlyCollection<BetItem> items, decimal payout)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            Items = items ?? new List<BetItem>().AsReadOnly();
            Payout = payout;
        }

        public void AddBetItem(BetItem item)
        {
            var items = Items.ToList();
            items.Add(item);
            Items = items.AsReadOnly();
        }

    }
}
