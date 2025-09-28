using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IWalletService
    {
        Task DeductForBetAsync(Guid id, decimal amount, string betDescription);

        Task DepositAsync(Guid id, decimal amount);

        Task<Wallet> GetByIdAsync(Guid id);

    }
}
