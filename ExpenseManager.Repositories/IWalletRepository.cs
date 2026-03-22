using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Repositories
{
    public interface IWalletRepository
    {
        IEnumerable<WalletDBModel> GetWallets();
        WalletDBModel GetWallet(Guid walletId);
    }
}