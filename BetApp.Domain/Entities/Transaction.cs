using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; } // positive = credit, negative = debit
        public decimal BalanceBefore { get; private set; }
        public decimal BalanceAfter { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; } 


        private Transaction() { }

        public Transaction(Guid walletId, decimal amount, decimal balanceBefore, decimal balanceAfter, TransactionType type , string description= "")
        {
            //Id = Guid.NewGuid();
            WalletId = walletId;
            Amount = amount;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            TransactionType = type;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
