using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.Pages;
using ExpenseManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpenseManager.ViewModels
{
    public partial class WalletDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        [ObservableProperty]
        private WalletDetailsDTO _currentWallet;

        [ObservableProperty]
        private ObservableCollection<TransactionListDTO> _transactions;

        public WalletDetailsViewModel(IWalletService walletService, ITransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var walletId = (Guid)query["WalletId"];
            CurrentWallet = _walletService.GetWallet(walletId);
            Transactions = new ObservableCollection<TransactionListDTO>(_transactionService.GetTransactionsByWalletId(walletId));
            OnPropertyChanged(nameof(Transactions));
        }

        [RelayCommand]
        private async Task LoadTransaction(Guid transactionId)
        {
            var transaction = Transactions.FirstOrDefault(t => t.Id == transactionId);
            if (transaction == null)
                return;

            await Shell.Current.Navigation.PushAsync(new TransactionDetailsPage(transaction));
        }
    }
}