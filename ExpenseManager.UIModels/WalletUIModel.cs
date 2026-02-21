using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;
using ExpenseManager.Services;
using ExpenseManager.UIModels;

namespace ExpenseManager.UIModels
{

    public class WalletUIModel
    {
        private string _name;
        private Valuta _valuta;
        private WalletDBModel _dbModel;
        private List<TransactionUIModel> _transactions;
        public Guid? Id 
        { 
            get => _dbModel?.Id; 
        }
        public string Name 
        {
            get => _name;
            set => _name = value;
                }
        public Valuta Valuta 
        {
            get => _valuta;
            set => _valuta = value;
        }
        public IReadOnlyList<TransactionUIModel> Transactions 
        {
            get => _transactions;
                 
        }
        public decimal TotalAmount => _transactions.Sum(t => t.Amount);
        public WalletUIModel()
        {
            _transactions = new List<TransactionUIModel>();
        }
        public WalletUIModel (WalletDBModel dbModel) : this()
        {
            _dbModel = dbModel;
            _name = dbModel.Name;
            _valuta = dbModel.Valuta;
        }

        public void SaveChangesToDBModel()
        {
            if (_dbModel != null)
            {
                _dbModel.Name = _name;
                _dbModel.Valuta = _valuta;
            }
            else
            {
                _dbModel = new WalletDBModel(_name, _valuta);
            }
        }
        public void LoadTransactions(StorageServices storage)
        {
            if (Id == null || _transactions.Count > 0)
                return;

            foreach (var txDb in storage.GetTransactionsByWalletId(Id.Value))
            {
                _transactions.Add(new TransactionUIModel(txDb));
            }
        }

        public override string ToString()
        {
            return $"Wallet Name: {Name}, Valuta: {Valuta}, TotalAmount: {TotalAmount}";
        }
    }
}
