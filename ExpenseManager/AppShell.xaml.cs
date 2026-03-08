using ExpenseManager.Pages;

namespace ExpenseManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}", typeof(WalletDetailsPage));
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}/{nameof(TransactionDetailsPage)}", typeof(TransactionDetailsPage));
        }
    }
}
