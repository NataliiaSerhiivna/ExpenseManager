using ExpenseManager.DTOModels.Wallets;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Services
{
    public interface IWalletService
    {
        IEnumerable<WalletListDTO> GetAllWallets();
        WalletDetailsDTO GetWallet(Guid walletId);
    }
}