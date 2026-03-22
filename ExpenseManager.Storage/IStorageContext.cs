using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Storage
{
    public interface IStorageContext
    {
        IEnumerable<WalletDBModel> GetWallets();
        WalletDBModel GetWallet(Guid walletId);
        IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId);
        int GetTransactionsCountByWalletId(Guid walletId);
        TransactionDBModel GetTransaction(Guid transactionId);
    }
}
