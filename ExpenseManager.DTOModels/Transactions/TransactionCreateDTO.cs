using ExpenseManager.Common.Enums;

namespace ExpenseManager.DTOModels.Transactions
{
    public class TransactionCreateDTO
    {
        public Guid WalletId { get; }

        public Category Category { get; }

        public decimal Amount { get; }
        public string Description { get; }

        public DateTime Timestamp { get; }


        public TransactionCreateDTO(Guid walletId, Category category, decimal amount, string description, DateTime timestamp)
        {
            WalletId = walletId;
            Category = category;
            Amount = amount;
            Description = description;
            Timestamp = timestamp;

        }
    }
}