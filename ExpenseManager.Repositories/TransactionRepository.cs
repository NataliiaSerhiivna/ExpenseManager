using ExpenseManager.DBModels;
using ExpenseManager.Storage;

namespace ExpenseManager.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IStorageContext _storageContext;

        public TransactionRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId)
        {
            return _storageContext.GetTransactionsByWalletAsync(walletId);
        }

        public Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId)
        {
            return _storageContext.GetTransactionAsync(transactionId);
        }

        public Task<int> GetTransactionsCountByWalletAsync(Guid walletId)
        {
            return _storageContext.GetTransactionsCountByWalletAsync(walletId);
        }

        public Task SaveTransactionAsync(TransactionDBModel transaction)
        {
            return _storageContext.SaveTransactionAsync(transaction);
        }

        public Task DeleteTransactionAsync(Guid transactionId)
        {
            return _storageContext.DeleteTransactionAsync(transactionId);
        }
    }
}