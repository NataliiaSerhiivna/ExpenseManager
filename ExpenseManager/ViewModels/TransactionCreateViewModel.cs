
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Services;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.ViewModels
{
    public partial class TransactionCreateViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITransactionService _transactionService;

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

        public TransactionCreateViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _categories = EnumExtensions.GetValueWithNames<Category>();
            Errors = InitErrors();
            Timestamp = DateTime.Today;
            Description = string.Empty;
            Amount = string.Empty;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _walletId = (Guid)query[nameof(TransactionCreateDTO.WalletId)];
        }

        [RelayCommand]
        public async Task CreateTransaction()
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
                var newTransaction = new TransactionCreateDTO(
                    _walletId,
                    Category.Value,
                    parsedAmount!.Value,
                    Description,
                    Timestamp!.Value
                    );

                await _transactionService.CreateTransactionAsync(newTransaction);

                await Shell.Current.DisplayAlertAsync(
                    "Success",
                    "Transaction created successfully!",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to create transaction: {ex.Message}",
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