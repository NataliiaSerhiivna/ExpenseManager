using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class TransactionDetailsPage : ContentPage
{
    private readonly TransactionDetailsViewModel _viewModel;

    public TransactionDetailsPage(TransactionDetailsViewModel vm)
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