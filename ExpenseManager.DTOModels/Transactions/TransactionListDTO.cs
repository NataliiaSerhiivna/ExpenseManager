using ExpenseManager.Common.Enums;
using System;

namespace ExpenseManager.DTOModels.Transactions
{
    public class TransactionListDTO
    {
        public Guid Id { get; }
        public decimal Amount { get; }
        public Category Category { get; }
        public string Description { get; }
        public DateTime Timestamp { get; }

        public TransactionListDTO(Guid id, decimal amount, Category category, string description, DateTime timestamp)
        {
            Id = id;
            Amount = amount;
            Category = category;
            Description = description;
            Timestamp = timestamp;
        }
    }
}