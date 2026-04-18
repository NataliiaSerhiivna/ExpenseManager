using ExpenseManager.DTOModels.Transactions;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Services
{
    public interface ITransactionService
    {
        //Task<IEnumerable<TransactionListDTO>> GetTransactionsByWalletIdAsync(Guid walletId);
        Task<IEnumerable<TransactionListDTO>> GetTransactionsByWalletAsync(Guid walletId);

        Task<TransactionDetailsDTO?> GetTransactionAsync(Guid transactionId);

        Task CreateTransactionAsync(TransactionCreateDTO transactionCreateDTO);
        Task UpdateTransactionAsync(TransactionEditDTO transactionEditDTO);

        Task DeleteTransactionAsync(Guid transactionId);
    }
}