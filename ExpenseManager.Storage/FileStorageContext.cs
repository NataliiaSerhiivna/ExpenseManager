using ExpenseManager.DBModels;
using Microsoft.Maui.Storage;
using System.Text.Json;

namespace ExpenseManager.Storage
{
    public class FileStorageContext : IStorageContext
    {
        private static readonly string DatabasePath =
            Path.Combine(FileSystem.AppDataDirectory, "ExpenseManagerStorage");

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private async Task Init()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (!Directory.Exists(DatabasePath))
                    await CreateMockStorageAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task CreateMockStorageAsync()
        {
            Directory.CreateDirectory(DatabasePath);

            var inMemoryStorage = new InMemoryStorageContext();
            var tasks = new List<Task>();

            await foreach (var wallet in inMemoryStorage.GetWalletsAsync())
            {
                Directory.CreateDirectory(WalletDirectoryPath(wallet.Id));

                tasks.Add(File.WriteAllTextAsync(
                    WalletFilePath(wallet.Id),
                    JsonSerializer.Serialize(wallet)));

                foreach (var transaction in await inMemoryStorage.GetTransactionsByWalletAsync(wallet.Id))
                {
                    tasks.Add(File.WriteAllTextAsync(
                        TransactionFilePath(wallet.Id, transaction.Id),
                        JsonSerializer.Serialize(transaction)));
                }
            }

            await Task.WhenAll(tasks);
        }
        

        private string WalletFilePath(Guid walletId)
        {
            return Path.Combine(DatabasePath, walletId + ".json");
        }

        private string WalletDirectoryPath(Guid walletId)
        {
            return Path.Combine(DatabasePath, walletId.ToString());
        }

        private string TransactionFilePath(Guid walletId, Guid transactionId)
        {
            return TransactionFilePath(WalletDirectoryPath(walletId), transactionId);
        }

        private string TransactionFilePath(string walletFolderPath, Guid transactionId)
        {
            return Path.Combine(walletFolderPath, transactionId + ".json");
        }

        public async IAsyncEnumerable<WalletDBModel> GetWalletsAsync()
        {
            await Init();

            foreach (var file in Directory.GetFiles(DatabasePath, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                var wallet = JsonSerializer.Deserialize<WalletDBModel>(json);

                if (wallet is not null)
                    yield return wallet;
            }
        }

        public async Task<WalletDBModel?> GetWalletAsync(Guid walletId)
        {
            await Init();

            var filePath = WalletFilePath(walletId);
            if (!File.Exists(filePath))
                return null;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<WalletDBModel>(json);
        }

        public async Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId)
        {
            await Init();

            var transactions = new List<TransactionDBModel>();
            var walletDirectory = WalletDirectoryPath(walletId);

            if (!Directory.Exists(walletDirectory))
                return transactions;

            foreach (var file in Directory.GetFiles(walletDirectory, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                var transaction = JsonSerializer.Deserialize<TransactionDBModel>(json);

                if (transaction is not null)
                    transactions.Add(transaction);
            }

            return transactions;
        }

        public async Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId)
        {
            await Init();

            foreach (var directory in Directory.GetDirectories(DatabasePath))
            {
                var filePath = TransactionFilePath(directory, transactionId);
                if (!File.Exists(filePath))
                    continue;

                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<TransactionDBModel>(json);
            }

            return null;
        }

        public async Task<int> GetTransactionsCountByWalletAsync(Guid walletId)
        {
            await Init();

            var walletDirectory = WalletDirectoryPath(walletId);
            if (!Directory.Exists(walletDirectory))
                return 0;

            return Directory.GetFiles(walletDirectory, "*.json").Length;
        }

        public async Task SaveWalletAsync(WalletDBModel wallet)
        {
            await Init();

            var walletDirectory = WalletDirectoryPath(wallet.Id);
            if (!Directory.Exists(walletDirectory))
                Directory.CreateDirectory(walletDirectory);

            var filePath = WalletFilePath(wallet.Id);
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(wallet));
        }

        public async Task DeleteWalletAsync(Guid walletId)
        {
            await Init();

            var walletFilePath = WalletFilePath(walletId);
            if (File.Exists(walletFilePath))
                File.Delete(walletFilePath);

            var walletDirectory = WalletDirectoryPath(walletId);
            if (Directory.Exists(walletDirectory))
                Directory.Delete(walletDirectory, true);
        }

        public async Task SaveTransactionAsync(TransactionDBModel transaction)
        {
            await Init();

            var walletDirectory = WalletDirectoryPath(transaction.WalletId);
            if (!Directory.Exists(walletDirectory))
                Directory.CreateDirectory(walletDirectory);

            var filePath = TransactionFilePath(walletDirectory, transaction.Id);
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(transaction));
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            await Init();

            foreach (var directory in Directory.GetDirectories(DatabasePath))
            {
                var filePath = TransactionFilePath(directory, transactionId);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return;
                }
            }
        }
    }
}