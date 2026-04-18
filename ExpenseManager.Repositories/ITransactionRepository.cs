using ExpenseManager.DBModels;

namespace ExpenseManager.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId);

        Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId);

        Task<int> GetTransactionsCountByWalletAsync(Guid walletId);

        Task SaveTransactionAsync(TransactionDBModel transaction);

        Task DeleteTransactionAsync(Guid transactionId);
    }
}