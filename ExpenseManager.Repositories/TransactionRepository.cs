using ExpenseManager.DBModels;
using ExpenseManager.Storage;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IStorageContext _storageContext;

        public TransactionRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId)
        {
            return _storageContext.GetTransactionsByWalletId(walletId);
        }

        public int GetTransactionsCountByWalletId(Guid walletId)
        {
            return _storageContext.GetTransactionsCountByWalletId(walletId);
        }
    }
}