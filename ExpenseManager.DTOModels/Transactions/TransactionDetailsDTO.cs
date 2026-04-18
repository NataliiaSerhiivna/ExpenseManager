using ExpenseManager.Common.Enums;

namespace ExpenseManager.DTOModels.Transactions
{
    public class TransactionDetailsDTO
    {
        public Guid Id { get; }

        public Category Category { get; }

        public decimal Amount { get; }

        public DateTime Timestamp { get; }

        public string Description { get; }

        public TransactionDetailsDTO(Guid id, Category category, decimal amount, string description, DateTime timestamp)
        {
            Id = id;
            Category = category;
            Amount = amount;
            Description = description;
            Timestamp = timestamp;
        }
    }
}