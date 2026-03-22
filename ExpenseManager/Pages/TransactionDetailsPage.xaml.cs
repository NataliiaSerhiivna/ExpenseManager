using ExpenseManager.DTOModels.Transactions;

namespace ExpenseManager.Pages;

public partial class TransactionDetailsPage : ContentPage
{
    public TransactionDetailsPage(TransactionListDTO transaction)
    {
        InitializeComponent();
        BindingContext = transaction;
    }
}