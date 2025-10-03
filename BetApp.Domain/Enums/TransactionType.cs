using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Enums
{
    public enum TransactionType
    {
        Deposit = 1,
        BetPlaced = 2,
        Refund = 3,
        Bonus = 4
    }
}
