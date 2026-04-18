using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class WalletCreatePage : ContentPage
{
    public WalletCreatePage(WalletCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}