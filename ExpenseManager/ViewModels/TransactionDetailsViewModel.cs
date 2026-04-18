using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Pages;
using ExpenseManager.Services;

namespace ExpenseManager.ViewModels
{
    public partial class TransactionDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITransactionService _transactionService;

        private TransactionDetailsDTO _currentTransaction;
        private bool _isExpense;

        private Guid _transactionId;
        private Guid _walletId;

        public Category? Category => _currentTransaction?.Category;
        public decimal? Amount => _currentTransaction?.Amount;
        public DateTime? Timestamp => _currentTransaction?.Timestamp;
        public string Description => _currentTransaction?.Description;
        public bool IsExpense => _isExpense;
        public string ExpenseStatus => IsExpense ? "Yes" : "No";

        public TransactionDetailsViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _transactionId = (Guid)query["TransactionId"];
            if (query.ContainsKey("WalletId"))
                _walletId = (Guid)query["WalletId"];
        }

        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                _currentTransaction = await _transactionService.GetTransactionAsync(_transactionId)
                    ?? throw new Exception("Transaction does not exist.");

                CalculateIsExpense();

                OnPropertyChanged(nameof(Category));
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(Timestamp));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsExpense));
                OnPropertyChanged(nameof(ExpenseStatus));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to load transaction details: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task EditTransaction()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(
                    nameof(TransactionEditPage),
                    new Dictionary<string, object>
                    {
                        { "TransactionId", _transactionId },
                        { "WalletId", _walletId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to navigate to transaction edit page: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DeleteTransaction()
        {
            IsBusy = true;
            try
            {
                bool confirmed = await Shell.Current.DisplayAlertAsync(
                    "Confirm",
                    "Are you sure you want to delete this transaction?",
                    "Yes",
                    "No");

                if (!confirmed)
                {
                    IsBusy = false;
                    return;
                }

                await _transactionService.DeleteTransactionAsync(_transactionId);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to delete transaction: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to navigate back: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculateIsExpense()
        {
            if (Amount == null)
                return;

            _isExpense = Amount.Value < 0;
        }
    }
}