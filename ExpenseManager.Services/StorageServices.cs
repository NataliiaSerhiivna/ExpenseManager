using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;

namespace ExpenseManager.Services
{
    public class StorageServices
    {
        private List<WalletDBModel> _wallet;
        private List<TransactionDBModel> _transactions;

        //Lazy loading of data from artificial storage
        private void LoadData()
        {
            if (_wallet != null && _transactions != null)
                return;
            _wallet = FakeStorage.Wallets.ToList();
            _transactions = FakeStorage.Transactions.ToList();
        }
        
        //Gets all transactions of specified wallet
        public IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId)
        {
            LoadData();

            var result = new List<TransactionDBModel>();
            foreach (var transaction in _transactions)
            {
                if (transaction.WalletId == walletId)
                {
                    result.Add(transaction);
                }
            }

            return result;
        }

        //returns all wallets from the vault
        public IEnumerable<WalletDBModel> GetAllWallets()
        {
            LoadData();
            return _wallet.ToList();
        }
    }
}
