using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Services;
using ExpenseManager.UIModels;

namespace ExpenseManager.Pages;

[QueryProperty(nameof(CurrentWallet), nameof(CurrentWallet))]
public partial class WalletDetailsPage : ContentPage
{
    private IStorageService _storage;
    private WalletUIModel _currentWallet;

        public WalletUIModel CurrentWallet
    {
        get => _currentWallet;
        set
        {
            _currentWallet = value;
            _currentWallet.LoadTransactions();
            BindingContext = CurrentWallet;
        }
    }
    public WalletDetailsPage(IStorageService storage)
    {
        InitializeComponent();
        _storage = storage;
    }
    private void TransactionSelected(object sender, SelectionChangedEventArgs e)
    {
        var transaction = (TransactionUIModel)e.CurrentSelection[0];
        Shell.Current.GoToAsync($"{nameof(TransactionDetailsPage)}", new Dictionary<string, object> { { nameof(TransactionDetailsPage.CurrentTransaction), transaction } });
    }

}
