using ExpenseManager.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.Services
{
    [Obsolete("This class was created for testing and learning purposes. It is no longer needed and will be removed in the future.")]
    public interface IStorageService
    {
        IEnumerable<WalletDBModel> GetAllWallets();
        IEnumerable<TransactionDBModel> GetTransactionsByWalletId(Guid walletId);
    }
}
