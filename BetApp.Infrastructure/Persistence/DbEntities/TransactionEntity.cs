using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class TransactionEntity
    {
        public long Id {  get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string Description { get; set; } = string.Empty;
        public int TransactionType { get; set; }
        public WalletEntity Wallet { get;  set; } 
    }
}
