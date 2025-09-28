using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public decimal Amount { get; set; } // positive = credit, negative = debit
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
    }
}
