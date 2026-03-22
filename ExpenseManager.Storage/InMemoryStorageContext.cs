using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;
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
            _transactions.Add(new TransactionRecord(Guid.NewGuid(), paypalWallet.Id, -300m, Category.Health, "Pharmacy", new DateTime(2026, 1, 9, 16, 20, 0)));
            #endregion
        }

        public IEnumerable<WalletDBModel> GetWallets()
        {
            foreach (var wallet in _wallets)
            {
                yield return new WalletDBModel(wallet.Id, wallet.Name, wallet.Valuta);
            }
        }

        public WalletDBModel GetWallet(Guid walletId)
        {
            var wallet = _wallets.FirstOrDefault(wallet => wallet.Id == walletId);
            return wallet is null
                ? null
                : new WalletDBModel(wallet.Id, wallet.Name, wallet.Valuta);
        }

        public IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId)
        {
            return _transactions
                .Where(transaction => transaction.WalletId == walletId)
                .Select(transaction => new TransactionDBModel(
                    transaction.Id,
                    transaction.WalletId,
                    transaction.Amount,
                    transaction.Category,
                    transaction.Description,
                    transaction.Timestamp));
        }
        public TransactionDBModel GetTransaction(Guid transactionId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);

            return transaction is null
                ? null
                : new TransactionDBModel(
                    transaction.Id,
                    transaction.WalletId,
                    transaction.Amount,
                    transaction.Category,
                    transaction.Description,
                    transaction.Timestamp
                );
        }

        public int GetTransactionsCountByWalletId(Guid walletId)
        {
            return _transactions.Count(transaction => transaction.WalletId == walletId);
        }
    }
}
