using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.Services
{
    public interface IStorageService
    {
        IEnumerable<WalletDBModel> GetAllWallets();
        IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId);
    }
}
