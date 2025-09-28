using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class WalletEntity
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public ICollection<TransactionEntity> Transactions { get; set; } = null!;
    }
}
