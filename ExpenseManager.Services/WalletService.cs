using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseManager.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletService(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public IEnumerable<WalletListDTO> GetAllWallets()
        {
            foreach (var wallet in _walletRepository.GetWallets())
            {
                var transactions = _transactionRepository.GetTransactionsByWalletId(wallet.Id).ToList();
                var totalAmount = transactions.Sum(transaction => transaction.Amount);

                yield return new WalletListDTO(wallet.Id, wallet.Name, wallet.Valuta, totalAmount);
            }
        }

        public WalletDetailsDTO GetWallet(Guid walletId)
        {
            var wallet = _walletRepository.GetWallet(walletId);
            return wallet is null
                ? null
                : new WalletDetailsDTO(wallet.Id, wallet.Name, wallet.Valuta);
        }

    }
}