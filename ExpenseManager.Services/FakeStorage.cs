using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;



namespace ExpenseManager.Services
{
    internal static class FakeStorage
    {
        private static readonly List<WalletDBModel> _wallet;
        private static readonly List<TransactionDBModel> _transactions;

        internal static IEnumerable<WalletDBModel> Wallets
        {
            get
            {
                return _wallet.ToList();
            }
        }

        internal static IEnumerable<TransactionDBModel> Transactions
        {
            get
            {
                return _transactions.ToList();
            }
        }

        static FakeStorage()
        {
            var walletOfUAH = new WalletDBModel("WalletUAH", Valuta.UAH);
            var walletOfUSD = new WalletDBModel("WalletUSD", Valuta.USD);
            var walletOfEUR = new WalletDBModel("WalletEUR", Valuta.EUR);
            var walletOfJPY = new WalletDBModel("WalletJPY", Valuta.JPY);
            _wallet = new List<WalletDBModel> { walletOfUAH, walletOfUSD, walletOfEUR, walletOfJPY };
            _transactions = new List<TransactionDBModel>
            {
                new TransactionDBModel(walletOfUAH.Id, 15000m, Category.Products, "Salary", new DateTime(2026, 1, 5, 9, 0, 0)),
                new TransactionDBModel(walletOfUAH.Id, -320m, Category.Products, "ATB", new DateTime(2026, 1, 6, 18, 30, 0)),
                new TransactionDBModel(walletOfUAH.Id, -110m, Category.Cafe, "Coffee", new DateTime(2026, 1, 7, 10, 15, 0)),
                new TransactionDBModel(walletOfUAH.Id, -850m, Category.Home, "Internet", new DateTime(2026, 1, 8, 12, 0, 0)),
                new TransactionDBModel(walletOfUAH.Id, -300m, Category.Health, "Pharmacy", new DateTime(2026, 1, 9, 16, 20, 0)),
                new TransactionDBModel(walletOfUAH.Id, -270m, Category.Products, "Silpo", new DateTime(2026, 1, 10, 19, 0, 0)),
                new TransactionDBModel(walletOfUAH.Id, -90m, Category.Cafe, "Bakery", new DateTime(2026, 1, 11, 8, 15, 0)),
                new TransactionDBModel(walletOfUAH.Id, -420m, Category.Home, "Utilities", new DateTime(2026, 1, 12, 11, 30, 0)),
                new TransactionDBModel(walletOfUAH.Id, -150m, Category.Health, "Vitamins", new DateTime(2026, 1, 13, 17, 45, 0)),
                new TransactionDBModel(walletOfUAH.Id, -500m, Category.Products, "Household goods", new DateTime(2026, 1, 14, 20, 0, 0)),
                new TransactionDBModel(walletOfUSD.Id, 2000m, Category.Products, "Freelance payment", new DateTime(2026, 1, 15, 14, 0, 0)),
                new TransactionDBModel(walletOfUSD.Id, -150m, Category.Home, "Hosting payment", new DateTime(2026, 1, 16, 9, 30, 0)),
                new TransactionDBModel(walletOfEUR.Id, -300m, Category.Health, "Pharmacy", new DateTime(2026, 1, 9, 16, 20, 0))
            };

        }
    }
}
