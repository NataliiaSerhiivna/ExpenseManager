using ExpenseManager.Pages;

namespace ExpenseManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}", typeof(WalletDetailsPage));
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletCreatePage)}", typeof(WalletCreatePage));
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}/{nameof(WalletEditPage)}", typeof(WalletEditPage));
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}/{nameof(TransactionDetailsPage)}", typeof(TransactionDetailsPage));
            Routing.RegisterRoute($"{nameof(WalletsPage)}/{nameof(WalletDetailsPage)}/{nameof(TransactionCreatePage)}", typeof(TransactionCreatePage));
            //Routing.RegisterRoute(nameof(WalletDetailsPage), typeof(WalletDetailsPage));
            Routing.RegisterRoute(nameof(WalletCreatePage), typeof(WalletCreatePage));
            Routing.RegisterRoute(nameof(WalletEditPage), typeof(WalletEditPage));
            Routing.RegisterRoute(nameof(TransactionDetailsPage), typeof(TransactionDetailsPage));
            Routing.RegisterRoute(nameof(TransactionCreatePage), typeof(TransactionCreatePage));
            Routing.RegisterRoute(nameof(TransactionDetailsPage), typeof(TransactionDetailsPage));
            Routing.RegisterRoute(nameof(TransactionEditPage), typeof(TransactionEditPage));
        }
    }
}
