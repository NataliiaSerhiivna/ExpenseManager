using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Pages;
using ExpenseManager.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpenseManager.ViewModels
{
    public partial class WalletDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        private Guid _walletId;
        private decimal _totalAmount;
        private List<TransactionListDTO> _allTransactions = new List<TransactionListDTO>();

        public decimal TotalAmount => _totalAmount;

        [ObservableProperty]
        private WalletDetailsDTO _currentWallet;

        [ObservableProperty]
        private ObservableCollection<TransactionListDTO> _transactions;

        [ObservableProperty]
        private string _transactionSearchText;

        [ObservableProperty]
        private Category? _selectedCategoryFilter;

        [ObservableProperty]
        private TransactionSortOption _selectedTransactionSortOption;

        public Category?[] CategoryFilters =>
            new Category?[] { null }
            .Concat(Enum.GetValues<Category>().Select(category => (Category?)category))
            .ToArray();

        public TransactionSortOption[] TransactionSortOptions =>
            Enum.GetValues<TransactionSortOption>();

        public WalletDetailsViewModel(
            IWalletService walletService,
            ITransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;

            Transactions = new ObservableCollection<TransactionListDTO>();
            TransactionSearchText = string.Empty;
            SelectedTransactionSortOption = TransactionSortOption.ByTimestampDescending;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _walletId = (Guid)query["WalletId"];
            OnPropertyChanged(nameof(Transactions));
        }

        partial void OnTransactionSearchTextChanged(string value)
        {
            ApplyTransactionFilters();
        }

        partial void OnSelectedCategoryFilterChanged(Category? value)
        {
            ApplyTransactionFilters();
        }

        partial void OnSelectedTransactionSortOptionChanged(TransactionSortOption value)
        {
            ApplyTransactionFilters();
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                CurrentWallet = await _walletService.GetWalletAsync(_walletId)
                    ?? throw new Exception("Wallet does not exist.");

                _allTransactions = (await _transactionService.GetTransactionsByWalletAsync(_walletId)).ToList();
                ApplyTransactionFilters();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to load wallet details: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyTransactionFilters()
        {
            IEnumerable<TransactionListDTO> query = _allTransactions;

            if (!string.IsNullOrWhiteSpace(TransactionSearchText))
            {
                query = query.Where(transaction =>
                    !string.IsNullOrWhiteSpace(transaction.Description) &&
                    transaction.Description.Contains(TransactionSearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedCategoryFilter != null)
            {
                query = query.Where(transaction => transaction.Category == SelectedCategoryFilter.Value);
            }

            query = SelectedTransactionSortOption switch
            {
                TransactionSortOption.ByTimestampAscending => query.OrderBy(transaction => transaction.Timestamp),
                TransactionSortOption.ByAmountDescending => query.OrderByDescending(transaction => transaction.Amount),
                TransactionSortOption.ByAmountAscending => query.OrderBy(transaction => transaction.Amount),
                _ => query.OrderByDescending(transaction => transaction.Timestamp)
            };

            var filteredTransactions = query.ToList();

            Transactions = new ObservableCollection<TransactionListDTO>(filteredTransactions);
            _totalAmount = filteredTransactions.Sum(transaction => transaction.Amount);
            OnPropertyChanged(nameof(TotalAmount));
        }

        [RelayCommand]
        private async Task LoadTransaction(Guid transactionId)
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(TransactionDetailsPage)}",
                    new Dictionary<string, object>
                    {
                        { "TransactionId", transactionId },
                        { "WalletId", _walletId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to navigate to transaction details: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddTransaction()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(TransactionCreatePage)}",
                    new Dictionary<string, object>
                    {
                        { nameof(TransactionCreateDTO.WalletId), _walletId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to navigate to transaction create page: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteTransaction(TransactionListDTO transaction)
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
                    return;

                await _transactionService.DeleteTransactionAsync(transaction.Id);

                _allTransactions.RemoveAll(t => t.Id == transaction.Id);
                ApplyTransactionFilters();
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
        private async Task EditWallet()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(
                    nameof(WalletEditPage),
                    new Dictionary<string, object>
                    {
                        { "WalletId", _walletId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate to wallet edit page: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteWallet()
        {
            IsBusy = true;
            try
            {
                bool confirmed = await Shell.Current.DisplayAlertAsync(
                    "Confirm",
                    "Are you sure you want to delete this wallet?",
                    "Yes",
                    "No");

                if (!confirmed)
                    return;

                await _walletService.DeleteWalletAsync(_walletId);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete wallet: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}