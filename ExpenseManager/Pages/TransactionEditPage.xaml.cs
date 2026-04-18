using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class TransactionEditPage : ContentPage
{
    private readonly TransactionEditViewModel _viewModel;

    public TransactionEditPage(TransactionEditViewModel vm)
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