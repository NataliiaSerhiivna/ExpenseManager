using ExpenseManager.DTOModels.Transactions;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Services
{
    public interface ITransactionService
    {
        IEnumerable<TransactionListDTO> GetTransactionsByWalletId(Guid walletId);
    }
}