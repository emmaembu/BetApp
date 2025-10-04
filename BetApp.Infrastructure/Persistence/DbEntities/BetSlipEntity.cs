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
        public decimal NetStake { get; set; }
        public decimal TotalStake { get; set; }
        public decimal FeePercent { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime PlacedAt { get; set; }
        public decimal Payout { get; set; }
        public ICollection<BetItemEntity> BetItems { get; set; } = null!;
    }
}
