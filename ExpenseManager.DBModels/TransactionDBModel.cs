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
        public Guid Id { get; set; }
        //The transaction belongs to one specific wallet
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        // Timestamp is set only once during the creation of the object and cannot be changed later.
        public DateTime Timestamp { get; set; }
        public TransactionDBModel()
        {
        }
        public TransactionDBModel(Guid walletId, decimal amount, Category category, string description, DateTime timestamp): this(Guid.NewGuid(), walletId, amount, category, description, timestamp){}
        public TransactionDBModel(Guid id, Guid walletId, decimal amount, Category category, string description, DateTime timestamp)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            Category = category;
            Description = description;
            Timestamp = timestamp;
        }
    }
}