using BetApp.Domain.Enums;
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
        public decimal Balance { get; private set; }    
        
        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
        public byte[] RowVersion { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private Wallet() { } // for EF

        public Wallet(Guid id)
        {
            Id = id;
            Balance = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        //add new founds
        public void Deposit(decimal amount, string description = "")
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            var balanceBefore = Balance;
            Balance += amount;
            _transactions.Add(new Transaction
                (
                walletId : Id,
                amount : amount,
                balanceBefore : balanceBefore,
                balanceAfter : Balance,
                type :TransactionType.Deposit,
                description: description
                ));

                UpdatedAt = DateTime.UtcNow;
        }

        // new BetSlip
        public void Deduct(decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (Balance < amount)
                throw new ArgumentException("Insufficient funds.");

            decimal before = Balance;
            Balance -= amount;
            _transactions.Add(new Transaction
            (
                walletId :Id,
                amount : -amount,
                balanceBefore: before,
                balanceAfter : Balance,
                type : TransactionType.BetPlaced,
                description : description
            ));
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
