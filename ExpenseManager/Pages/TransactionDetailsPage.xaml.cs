using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.UIModels;

namespace ExpenseManager.Pages;

[QueryProperty(nameof(CurrentTransaction), nameof(CurrentTransaction))]
public partial class TransactionDetailsPage : ContentPage
{
    public TransactionDetailsPage()
    {
        InitializeComponent();
    }

    private TransactionUIModel _currentTransaction;
    public TransactionUIModel CurrentTransaction
    {
        get => _currentTransaction;
        set
        {
            _currentTransaction = value;
            BindingContext = _currentTransaction;
        }
    }
}
