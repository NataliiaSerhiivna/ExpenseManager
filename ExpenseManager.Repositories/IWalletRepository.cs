using ExpenseManager.DBModels;

namespace ExpenseManager.Repositories
{
    public interface IWalletRepository
    {
        IAsyncEnumerable<WalletDBModel> GetWalletsAsync();
        Task<WalletDBModel?> GetWalletAsync(Guid walletId);
        Task SaveWalletAsync(WalletDBModel wallet);
        Task DeleteWalletAsync(Guid walletId);
    }
}