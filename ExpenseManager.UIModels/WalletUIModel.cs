using ExpenseManager.Common.Enums;
using ExpenseManager.DBModels;
using ExpenseManager.Services;

namespace ExpenseManager.UIModels
{
    [Obsolete("This class was created for testing and learning purposes. It is no longer needed and will be removed in the future.")]
    public class WalletUIModel
    {
        private readonly IStorageService _storage;
        private WalletDBModel _dbModel;
        private string _name;
        private Valuta _valuta;
        private List<TransactionUIModel>? _transactions;

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

        public IReadOnlyList<TransactionUIModel>? Transactions
        {
            get => _transactions;
        }

        public decimal TotalAmount
        {
            get => Transactions?.Sum(t => t.Amount) ?? -1;
        }

        public string TotalAmountDesc
        {
            get => TotalAmount == -1 ? "Not Loaded" : TotalAmount.ToString();
        }

        public WalletUIModel(IStorageService storage)
        {
            _storage = storage;
        }

        public WalletUIModel(IStorageService storage, WalletDBModel dbModel)
        {
            _storage = storage;
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

        public void LoadTransactions()
        {
            if (Id == null || _transactions != null)
                return;

            _transactions = new List<TransactionUIModel>();

            foreach (var transactionDb in _storage.GetTransactionsByWalletId(Id.Value))
            {
                _transactions.Add(new TransactionUIModel(transactionDb));
            }
        }

        public override string ToString()
        {
            return $"Wallet: {Name}, Valuta: {Valuta}, Total amount: {TotalAmountDesc}";
        }
    }
}