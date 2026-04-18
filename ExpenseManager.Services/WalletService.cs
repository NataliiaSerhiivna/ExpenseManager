using ExpenseManager.DBModels;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExpenseManager.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletService(
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async IAsyncEnumerable<WalletListDTO> GetAllWalletsAsync()
        {
            await foreach (var wallet in _walletRepository.GetWalletsAsync())
            {
                var transactions = await _transactionRepository.GetTransactionsByWalletAsync(wallet.Id);
                var totalAmount = transactions.Sum(transaction => transaction.Amount);

                yield return new WalletListDTO(
                    wallet.Id,
                    wallet.Name,
                    wallet.Valuta,
                    totalAmount);
            }
        }

        public async Task<WalletDetailsDTO?> GetWalletAsync(Guid walletId)
        {
            var wallet = await _walletRepository.GetWalletAsync(walletId);
            if (wallet is null)
                return null;

            var transactions = await _transactionRepository.GetTransactionsByWalletAsync(wallet.Id);
            var totalAmount = transactions.Sum(transaction => transaction.Amount);

            return new WalletDetailsDTO(
                wallet.Id,
                wallet.Name,
                wallet.Valuta,
                totalAmount);
        }

        public async Task CreateWalletAsync(WalletCreateDTO walletCreateDTO)
        {
            var errors = walletCreateDTO.Validate();
            if (errors.Count > 0)
                throw new ValidationException(string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage)));

            var wallet = new WalletDBModel(walletCreateDTO.Name, walletCreateDTO.Valuta);
            await _walletRepository.SaveWalletAsync(wallet);
        }

        public async Task UpdateWalletAsync(WalletEditDTO walletEditDTO)
        {
            var errors = walletEditDTO.Validate();
            if (errors.Count > 0)
                throw new ValidationException(string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage)));

            var wallet = new WalletDBModel(walletEditDTO.Id, walletEditDTO.Name, walletEditDTO.Valuta);
            await _walletRepository.SaveWalletAsync(wallet);
        }

        public Task DeleteWalletAsync(Guid walletId)
        {
            return _walletRepository.DeleteWalletAsync(walletId);
        }
    }

}
