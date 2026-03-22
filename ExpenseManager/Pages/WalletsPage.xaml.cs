using ExpenseManager.Services;
using ExpenseManager.UIModels;
using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class WalletsPage : ContentPage
{
    public WalletsPage(WalletsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}