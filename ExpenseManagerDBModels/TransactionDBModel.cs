using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;

namespace ExpenseManager.DBModels
{
    public class TransactionDBModel
    {
        //The identifier is immutable after the object is created
        public Guid Id { get; }
        //The transaction belongs to one specific wallet
        public Guid WalletId { get; }
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public TransactionDBModel(Guid walletId, decimal amount, Category category, string description, DateTime timestamp)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            Amount = amount;
            Category = category;
            Description = description ?? "";
            Timestamp = timestamp;
        }
    }
}
