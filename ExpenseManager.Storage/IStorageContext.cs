using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Storage
{
    public interface IStorageContext
    {
        IAsyncEnumerable<WalletDBModel> GetWalletsAsync();
        Task<WalletDBModel?> GetWalletAsync(Guid walletId);

        Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId);
        Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId);
        Task<int> GetTransactionsCountByWalletAsync(Guid walletId);

        Task SaveWalletAsync(WalletDBModel wallet);
        Task DeleteWalletAsync(Guid walletId);

        Task SaveTransactionAsync(TransactionDBModel transaction);
        Task DeleteTransactionAsync(Guid transactionId);
    }
}