using ExpenseManager.DBModels;
using ExpenseManager.Storage;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IStorageContext _storageContext;

        public WalletRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IEnumerable<WalletDBModel> GetWallets()
        {
            return _storageContext.GetWallets();
        }

        public WalletDBModel GetWallet(Guid walletId)
        {
            return _storageContext.GetWallet(walletId);
        }
    }
}