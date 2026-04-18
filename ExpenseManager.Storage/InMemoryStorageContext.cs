using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;
using System.Linq;

namespace ExpenseManager.Storage
{
    public class InMemoryStorageContext : IStorageContext
    {
        private record WalletRecord(Guid Id, string Name, Valuta Valuta);
        private record TransactionRecord(Guid Id, Guid WalletId, decimal Amount, Category Category, string Description, DateTime Timestamp);

        private static readonly List<WalletRecord> _wallets = new List<WalletRecord>();
        private static readonly List<TransactionRecord> _transactions = new List<TransactionRecord>();

        static InMemoryStorageContext()
        {
            #region MockStoragePopulation
            var cashWallet = new WalletRecord(Guid.NewGuid(), "Cash", Valuta.UAH);
            var monobankWallet = new WalletRecord(Guid.NewGuid(), "Monobank", Valuta.UAH);
            var paypalWallet = new WalletRecord(Guid.NewGuid(), "PayPal", Valuta.USD);

            _wallets.Add(cashWallet);
            _wallets.Add(monobankWallet);
            _wallets.Add(paypalWallet);

            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, 15000m, Category.Products, "Salary", new DateTime(2026, 1, 5, 9, 0, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -320m, Category.Products, "ATB", new DateTime(2026, 1, 6, 18, 30, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -110m, Category.Cafe, "Coffee", new DateTime(2026, 1, 7, 10, 15, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -850m, Category.Home, "Internet", new DateTime(2026, 1, 8, 12, 0, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -300m, Category.Health, "Pharmacy", new DateTime(2026, 1, 9, 16, 20, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -270m, Category.Products, "Silpo", new DateTime(2026, 1, 10, 19, 0, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -90m, Category.Cafe, "Bakery", new DateTime(2026, 1, 11, 8, 15, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -420m, Category.Home, "Utilities", new DateTime(2026, 1, 12, 11, 30, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -150m, Category.Health, "Vitamins", new DateTime(2026, 1, 13, 17, 45, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), monobankWallet.Id, -500m, Category.Products, "Household goods", new DateTime(2026, 1, 14, 20, 0, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), cashWallet.Id, 2000m, Category.Products, "Freelance payment", new DateTime(2026, 1, 15, 14, 0, 0)));
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), cashWallet.Id, -150m, Category.Home, "Hosting payment", new DateTime(2026, 1, 16, 9, 30, 0)));
            #endregion
        }

        public async IAsyncEnumerable<WalletDBModel> GetWalletsAsync()
        {
            foreach (var wallet in _wallets)
            {
                await Task.Delay(1000);
                yield return new WalletDBModel(wallet.Id, wallet.Name, wallet.Valuta);
            }
        }

        public Task<WalletDBModel?> GetWalletAsync(Guid walletId)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                var wallet = _wallets.FirstOrDefault(w => w.Id == walletId);
                return wallet is null ? null : new WalletDBModel(wallet.Id, wallet.Name, wallet.Valuta);
            });
        }

        public Task<IEnumerable<TransactionDBModel>> GetTransactionsByWalletAsync(Guid walletId)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                return _transactions
                    .Where(t => t.WalletId == walletId)
                    .Select(t => new TransactionDBModel(
                        t.Id,
                        t.WalletId,
                        t.Amount,
                        t.Category,
                        t.Description,
                        t.Timestamp));
            });
        }

        public Task<TransactionDBModel?> GetTransactionAsync(Guid transactionId)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
                return transaction is null
                    ? null
                    : new TransactionDBModel(
                        transaction.Id,
                        transaction.WalletId,
                        transaction.Amount,
                        transaction.Category,
                        transaction.Description,
                        transaction.Timestamp);
            });
        }

        public Task<int> GetTransactionsCountByWalletAsync(Guid walletId)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(500);
                return _transactions.Count(t => t.WalletId == walletId);
            });
        }

        public Task SaveWalletAsync(WalletDBModel wallet)
        {
            return Task.Run(() =>
            {
                var existing = _wallets.FirstOrDefault(w => w.Id == wallet.Id);
                if (existing is not null)
                    _wallets.Remove(existing);

                _wallets.Add(new WalletRecord(wallet.Id, wallet.Name, wallet.Valuta));
            });
        }

        public Task DeleteWalletAsync(Guid walletId)
        {
            return Task.Run(() =>
            {
                _wallets.RemoveAll(w => w.Id == walletId);
                _transactions.RemoveAll(t => t.WalletId == walletId);
            });
        }

        public Task SaveTransactionAsync(TransactionDBModel transaction)
        {
            return Task.Run(() =>
            {
                var existing = _transactions.FirstOrDefault(t => t.Id == transaction.Id);
                if (existing is not null)
                    _transactions.Remove(existing);

                _transactions.Add(new TransactionRecord(
                    transaction.Id,
                    transaction.WalletId,
                    transaction.Amount,
                    transaction.Category,
                    transaction.Description,
                    transaction.Timestamp));
            });
        }

        public Task DeleteTransactionAsync(Guid transactionId)
        {
            return Task.Run(() =>
            {
                _transactions.RemoveAll(t => t.Id == transactionId);
            });
        }
    }
}