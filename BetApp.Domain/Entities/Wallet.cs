using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        //add new founds
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            Balance += amount;
            Transactions.Add(new Transaction
            {
                WalletId = Id,
                Amount = amount,
                BalanceAfter = Balance,
                BalanceBefore = Balance - amount,
                Timestamp = DateTime.UtcNow,
                Description = "Deposit"
            });
        }

        // new BetSlip
        public void Deduct(decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (Balance - amount < 0)
                throw new ArgumentException("Insufficient funds.");

            decimal before = Balance;
            Balance -= amount;
            Transactions.Add(new Transaction
            {
                WalletId = Id,
                Amount = amount,
                BalanceAfter = Balance,
                BalanceBefore = Balance - amount,
                Timestamp = DateTime.UtcNow,
                Description = description
            });
        }
    }
}
