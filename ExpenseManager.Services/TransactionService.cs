using ExpenseManager.DBModels;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionListDTO>> GetTransactionsByWalletAsync(Guid walletId)
        {
            return (await _transactionRepository.GetTransactionsByWalletAsync(walletId))
                .Select(transaction => new TransactionListDTO(
                    transaction.Id,
                    transaction.Category,
                    transaction.Amount,
                    transaction.Description,
                    transaction.Timestamp));
        }

        public async Task<TransactionDetailsDTO?> GetTransactionAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionAsync(transactionId);

            return transaction is null
                ? null
                : new TransactionDetailsDTO(
                    transaction.Id,
                    transaction.Category,
                    transaction.Amount,
                    transaction.Description,
                    transaction.Timestamp);
        }
        public async Task UpdateTransactionAsync(TransactionEditDTO transactionEditDTO)
        {
            var errors = transactionEditDTO.Validate();
            if (errors.Count > 0)
                throw new ValidationException(
                    string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage)));

            var updatedTransaction = new TransactionDBModel(
                transactionEditDTO.Id,
                transactionEditDTO.WalletId,
                transactionEditDTO.Amount,
                transactionEditDTO.Category,
                transactionEditDTO.Description,
                transactionEditDTO.Timestamp);

            await _transactionRepository.SaveTransactionAsync(updatedTransaction);
        }

        public async Task CreateTransactionAsync(TransactionCreateDTO transactionCreateDTO)
        {
            var errors = transactionCreateDTO.Validate();

            if (errors.Count > 0)
                throw new ValidationException(
                    String.Join(Environment.NewLine,
                    errors.Select(e => e.ErrorMessage)));

            var newTransaction = new TransactionDBModel(
            transactionCreateDTO.WalletId,
            transactionCreateDTO.Amount,
            transactionCreateDTO.Category,
            transactionCreateDTO.Description,
            transactionCreateDTO.Timestamp);

            await _transactionRepository.SaveTransactionAsync(newTransaction);
        }

        public Task DeleteTransactionAsync(Guid transactionId)
        {
            return _transactionRepository.DeleteTransactionAsync(transactionId);
        }
    }
}