using ExpenseManager.DBModels;
using Microsoft.Maui.Storage;
using SQLite;

namespace ExpenseManager.Storage
{
    public class SQLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "expense_manager.db3";
        private static readonly string DatabasePath =
            Path.Combine(FileSystem.AppDataDirectory, "DB Storage 1", DatabaseFileName);

        private SQLiteAsyncConnection _databaseConnection;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private async Task Init()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_databaseConnection is not null)
                    return;

                bool isFirstLaunch = !File.Exists(DatabasePath);

                if (isFirstLaunch)
                    await CreateMockStorageAsync();
                else
                    _databaseConnection = new SQLiteAsyncConnection(DatabasePath);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task CreateMockStorageAsync()
        {
            var directoryPath = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            _databaseConnection = new SQLiteAsyncConnection(DatabasePath);

            var inMemoryStorage = new InMemoryStorageContext();

            await _databaseConnection.CreateTableAsync<WalletDBModel>();
            await _databaseConnection.CreateTableAsync<TransactionDBModel>();

            await foreach (var wallet in inMemoryStorage.GetWalletsAsync())
            {
                await _databaseConnection.InsertAsync(wallet);

                var walletTransactions = await inMemoryStorage.GetTransactionsByWalletAsync(wallet.Id);
                await _databaseConnection.InsertAllAsync(walletTransactions);
            }
        }

        public async IAsyncEnumerable<WalletDBModel> GetWalletsAsync()
        {
            await Init();

            foreach (var wallet in await _databaseConnection.Table<WalletDBModel>().ToListAsync())
            {
                yield return wallet;
            }
        }

        public async Task<WalletDBModel?> GetWalletAsync(Guid walletId)
        {
            await Init();
            return await _databaseConnection
                .Table<WalletDBModel>()
                .FirstOrDefaultAsync(wallet => wallet.Id == walletId);
        }

        public async Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId)
        {
            await Init();
            return await _databaseConnection
                .Table<TransactionDBModel>()
                .Where(transaction => transaction.WalletId == walletId)
                .ToListAsync();
        }

        public async Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId)
        {
            await Init();
            return await _databaseConnection
                .Table<TransactionDBModel>()
                .FirstOrDefaultAsync(transaction => transaction.Id == transactionId);
        }

        public async Task<int> GetTransactionsCountByWalletAsync(Guid walletId)
        {
            await Init();
            return await _databaseConnection
                .Table<TransactionDBModel>()
                .CountAsync(transaction => transaction.WalletId == walletId);
        }

        public async Task SaveWalletAsync(WalletDBModel wallet)
        {
            await Init();

            var existingWallet = await _databaseConnection
                .Table<WalletDBModel>()
                .FirstOrDefaultAsync(existing => existing.Id == wallet.Id);

            if (existingWallet is null)
                await _databaseConnection.InsertAsync(wallet);
            else
                await _databaseConnection.UpdateAsync(wallet);
        }

        public async Task DeleteWalletAsync(Guid walletId)
        {
            await Init();

            var walletTransactions = await _databaseConnection
                .Table<TransactionDBModel>()
                .Where(transaction => transaction.WalletId == walletId)
                .ToListAsync();

            foreach (var transaction in walletTransactions)
            {
                await _databaseConnection.DeleteAsync(transaction);
            }

            var wallet = await _databaseConnection
                .Table<WalletDBModel>()
                .FirstOrDefaultAsync(existing => existing.Id == walletId);

            if (wallet is not null)
                await _databaseConnection.DeleteAsync(wallet);
        }

        public async Task SaveTransactionAsync(TransactionDBModel transaction)
        {
            await Init();

            var existingTransaction = await _databaseConnection
                .Table<TransactionDBModel>()
                .FirstOrDefaultAsync(existing => existing.Id == transaction.Id);

            if (existingTransaction is null)
                await _databaseConnection.InsertAsync(transaction);
            else
                await _databaseConnection.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            await Init();

            var transaction = await _databaseConnection
                .Table<TransactionDBModel>()
                .FirstOrDefaultAsync(existing => existing.Id == transactionId);

            if (transaction is not null)
                await _databaseConnection.DeleteAsync(transaction);
        }
    }
}