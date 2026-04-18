using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Pages;
using ExpenseManager.Services;
using System.Globalization;


namespace ExpenseManager.ViewModels
{
    public partial class TransactionEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITransactionService _transactionService;

        private Guid _transactionId;
        private Guid _walletId;
        private EnumWithName<Category>[] _categories;

        [ObservableProperty]
        private string _amount;

        [ObservableProperty]
        private EnumWithName<Category> _category;

        [ObservableProperty]
        private DateTime? _timestamp;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private Dictionary<string, string> _errors;

        public EnumWithName<Category>[] Categories => _categories;

        public TransactionEditViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _categories = EnumExtensions.GetValueWithNames<Category>().ToArray();
            Errors = InitErrors();
            Timestamp = DateTime.Today;
            Description = string.Empty;
            Amount = string.Empty;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _transactionId = (Guid)query["TransactionId"];
            _walletId = (Guid)query["WalletId"];
        }

        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                var transaction = await _transactionService.GetTransactionAsync(_transactionId)
                    ?? throw new Exception("Transaction does not exist.");

                Amount = transaction.Amount.ToString(CultureInfo.InvariantCulture);
                Timestamp = transaction.Timestamp;
                Description = transaction.Description;
                Category = Categories.First(c => c.Value.Equals(transaction.Category));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to load transaction for edit: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task UpdateTransaction()
        {
            IsBusy = true;

            decimal? parsedAmount = null;
            if (!string.IsNullOrWhiteSpace(Amount) &&
                decimal.TryParse(Amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var amountValue))
            {
                parsedAmount = amountValue;
            }

            var errors = Validators.ValidateTransaction(
                parsedAmount,
                Category?.Value,
                Timestamp,
                Description);

            Errors = InitErrors();

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    if (string.IsNullOrWhiteSpace(Errors[error.MemberName]))
                    {
                        Errors[error.MemberName] = error.ErrorMessage;
                        continue;
                    }

                    Errors[error.MemberName] += Environment.NewLine + error.ErrorMessage;
                }

                OnPropertyChanged(nameof(Errors));
                IsBusy = false;
                return;
            }

            try
            {
                var updatedTransaction = new TransactionEditDTO(
                    _transactionId,
                    _walletId,
                    Category.Value,
                    parsedAmount!.Value,
                    Description,
                    Timestamp!.Value);

                await _transactionService.UpdateTransactionAsync(updatedTransaction);

                await Shell.Current.DisplayAlertAsync(
                    "Success",
                    "Transaction updated successfully!",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to update transaction: {ex.Message}",
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

        private Dictionary<string, string> InitErrors()
        {
            return new Dictionary<string, string>()
            {
                { nameof(Amount), string.Empty },
                { nameof(Category), string.Empty },
                { nameof(Timestamp), string.Empty },
                { nameof(Description), string.Empty }
            };
        }
    }
}