using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class WalletsPage : ContentPage
{
    private readonly WalletsViewModel _viewModel;

    public WalletsPage(WalletsViewModel vm)
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