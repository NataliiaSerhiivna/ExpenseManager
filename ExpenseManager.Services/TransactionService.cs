using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Repositories;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService()
        {
        }

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public IEnumerable<TransactionListDTO> GetTransactionsByWalletId(Guid walletId)
        {
            foreach (var transaction in _transactionRepository.GetTransactionsByWalletId(walletId))
            {
                yield return new TransactionListDTO(
                    transaction.Id,
                    transaction.Amount,
                    transaction.Category,
                    transaction.Description,
                    transaction.Timestamp
                );
            }
        }
      
    }
}