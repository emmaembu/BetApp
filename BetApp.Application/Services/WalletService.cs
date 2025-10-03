using AutoMapper;
using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet> CreateWalletAsync(decimal initialBalance)
        {
            var wallet = new Wallet(Guid.NewGuid());

            if(initialBalance > 0)
            {
                wallet.Deposit(initialBalance, "Initial deposit");
            }

            await _walletRepository.AddAsync(wallet);

            await _walletRepository.SaveChangesAsync();

            return wallet;
        }

        public async Task DepositAsync(WalletDepositDto request)
        {
            var wallet= await _walletRepository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                throw new InvalidOperationException("Wallet not found");
            }

            wallet.Deposit(request.Amount, request.Description);

            await _walletRepository.UpdateAsync(wallet);
        }

        public async Task DeductForBetAsync(Guid id, decimal amount, string betDescription)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
            {
                throw new InvalidOperationException("Wallet not found");
            }

            wallet.Deduct(amount, betDescription);

            await _walletRepository.UpdateAsync(wallet); 
        }

        public async Task<Wallet> GetByIdAsync(Guid id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
                throw new InvalidOperationException("Wallet not found");

            return wallet;
        }
    }
}
