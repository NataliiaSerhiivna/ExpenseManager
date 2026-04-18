using ExpenseManager.DBModels;
using ExpenseManager.Storage;

namespace ExpenseManager.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IStorageContext _storageContext;

        public WalletRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IAsyncEnumerable<WalletDBModel> GetWalletsAsync()
        {
            return _storageContext.GetWalletsAsync();
        }

        public Task<WalletDBModel?> GetWalletAsync(Guid walletId)
        {
            return _storageContext.GetWalletAsync(walletId);
        }

        public Task SaveWalletAsync(WalletDBModel wallet)
        {
            return _storageContext.SaveWalletAsync(wallet);
        }

        public Task DeleteWalletAsync(Guid walletId)
        {
            return _storageContext.DeleteWalletAsync(walletId);
        }
    }
}