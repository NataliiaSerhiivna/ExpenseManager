using ExpenseManager.ViewModels;

namespace ExpenseManager.Pages;

public partial class TransactionCreatePage : ContentPage
{
    public TransactionCreatePage(TransactionCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}