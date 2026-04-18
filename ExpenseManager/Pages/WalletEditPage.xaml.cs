using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class WalletEditPage : ContentPage
{
    private readonly WalletEditViewModel _viewModel;

    public WalletEditPage(WalletEditViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshData();
    }
}