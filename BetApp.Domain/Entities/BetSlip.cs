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
        public decimal FeePercent { get; private set; } = 0.05m; // 5%
        public decimal FeeAmount { get; private set; } //=> TotalStake*FeePercent;
        public decimal NetStake { get; private set; }//=> TotalStake - FeePercent;
        public DateTime PlacedAt { get; private set; } //{ get; set; } = DateTime.UtcNow;

        private readonly List<BetItem> _betItems = new();
        public IReadOnlyCollection<BetItem> BetItems => _betItems.AsReadOnly(); //{ get; private set; }

        public decimal TotalStake { get; private set; }//=> Items.Sum(i => i.Stake);
        public decimal Payout { get; private set; }

        private BetSlip() { }

        public BetSlip(Guid walletId, IEnumerable<BetItem> items)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            PlacedAt = DateTime.UtcNow;
            _betItems.AddRange(items);
            TotalStake = _betItems.Sum(i => i.Stake);
            FeeAmount = TotalStake * FeePercent;
            NetStake = TotalStake - FeeAmount;
            Payout = _betItems.Sum(i => i.Stake * i.OddsAtPlacement);

            //public void AddBetItem(BetItem item)
            //{
            //    var items = Items.ToList();
            //    items.Add(item);
            //    Items = items.AsReadOnly();
            //}

        }
    }
}
