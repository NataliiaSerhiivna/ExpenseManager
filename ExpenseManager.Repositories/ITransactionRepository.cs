using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId);
        int GetTransactionsCountByWalletId(Guid walletId);
    }
}