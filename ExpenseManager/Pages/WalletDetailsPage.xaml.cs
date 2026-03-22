using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Services;
using ExpenseManager.UIModels;
using ExpenseManager.ViewModels;
namespace ExpenseManager.Pages;

public partial class WalletDetailsPage : ContentPage
{
    public WalletDetailsPage(WalletDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}