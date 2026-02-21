using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;
using System.Xml.Linq;
using System.Collections;
using ExpenseManager.DBModels;
namespace ExpenseManager.UIModels
{
    public class TransactionUIModel
    { 
        private TransactionDBModel _dbModel;
        private Guid _walletId;
        private decimal _amount;
        private Category _category;
        private string _description;
        private DateTime _timestamp;
        private bool _isExpense;

        public Guid? Id
        {
            get => _dbModel?.Id;
        }
        public Guid WalletId
        {
            get => _walletId;
        }
        public decimal Amount
        {
            get => _amount;
            set => _amount = value;
        }
        public Category Category
        {
            get => _category;
            set => _category = value;
        }
        public string Description 
        {
            get => _description;
            set => _description = value;
        }
        public DateTime Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }
        public bool IsExpense => _amount < 0;
        public TransactionUIModel (Guid walletId)
        {
            _walletId = walletId;
        }
        public TransactionUIModel(TransactionDBModel dBModel)
        {
            _dbModel = dBModel;
            _walletId = dBModel.WalletId;
            _amount = dBModel.Amount;
            _category = dBModel.Category;
            _description = dBModel.Description;
            _timestamp = dBModel.Timestamp;
        }


        public void SaveChangesToDBModel()
        {
            if (_dbModel != null)
            {
                _dbModel.Amount = _amount;
                _dbModel.Category = _category;
                _dbModel.Description = _description;
                _dbModel.Timestamp = _timestamp;

            }
            else
            {
                _dbModel = new TransactionDBModel(_walletId, _amount, _category, _description, _timestamp);
            }
        }

        public override string ToString()
        {
            return $"Transaction: Amount: {Amount}, Category: {Category}, IsExpense: {IsExpense}, Date: {Timestamp:yyyy-MM-dd HH:mm}, Description: {Description}";
        }

    }
}
