using ExpenseManager.Common.Enums;

namespace ExpenseManager.DTOModels.Transactions
{
    public class TransactionEditDTO
    {
        public Guid Id { get; }
        public Guid WalletId { get; }
        public Category Category { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public DateTime Timestamp { get; }

        public TransactionEditDTO(
            Guid id,
            Guid walletId,
            Category category,
            decimal amount,
            string description,
            DateTime timestamp)
        {
            Id = id;
            WalletId = walletId;
            Category = category;
            Amount = amount;
            Description = description;
            Timestamp = timestamp;
        }
    }
}