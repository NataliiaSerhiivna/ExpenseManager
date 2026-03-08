using ExpenseManager.Services;
using ExpenseManager.UIModels;

namespace ExpenseManager.ConsoleApp
{
    internal class Program
    {
        private enum AppState
        {
            Default = 0,
            WalletDetails = 1,
            End = 2,
            Exit = 100,
        }

        private static AppState _appState = AppState.Default;
        private static StorageServices _storageService;
        private static List<WalletUIModel> _wallets;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello and welcome to the Expense Manager Console App!");

            _storageService = new StorageServices();

            string? command = null;
            while (_appState != AppState.Exit)
            {
                switch (_appState)
                {
                    case AppState.WalletDetails:
                        WalletDetailsState(command);
                        break;

                    case AppState.Default:
                        DefaultState();
                        break;
                }

                Console.WriteLine("Type Exit to close application.");
                command = Console.ReadLine();
                UpdateState(command);
            }
        }

        private static void UpdateState(string? command)
        {
            switch (command)
            {
                case "Back":
                    _appState = AppState.Default;
                    break;

                case "Refresh":
                    _wallets = null;
                    _appState = AppState.Default;
                    break;

                case "Exit":
                    _appState = AppState.Exit;
                    Console.WriteLine("Thank you and see you later!");
                    break;

                default:
                    switch (_appState)
                    {
                        case AppState.Default:
                            _appState = AppState.WalletDetails;
                            break;

                        case AppState.End:
                            Console.WriteLine("Unknown command. Please try again.");
                            break;
                    }
                    break;
            }
        }

        private static void DefaultState()
        {
            Console.WriteLine("\nHere is the list of all Wallets:");
            LoadWallets();

            foreach (var wallet in _wallets)
            {
                Console.WriteLine(wallet);
            }

            Console.WriteLine("Type the name of Wallet to see its Transactions.");
            Console.WriteLine("Example: WalletUAH");
            Console.WriteLine("Type Refresh to reload wallets.");
        }

        private static void LoadWallets()
        {
            if (_wallets != null)
                return;

            _wallets = new List<WalletUIModel>();

            foreach (var walletDb in _storageService.GetAllWallets())
            {
               // var walletUi = new WalletUIModel(walletDb);
                //_wallets.Add(walletUi);
            }
        }

        private static void WalletDetailsState(string? walletName)
        {
            bool walletExists = false;

            foreach (var wallet in _wallets)
            {
                if (wallet.Name == walletName)
                {
                    walletExists = true;

                    Console.WriteLine();
                    Console.WriteLine(wallet);

                    wallet.LoadTransactions();

                    Console.WriteLine($"Transactions in {wallet.Name}:");
                    if (wallet.Transactions.Count == 0)
                    {
                        Console.WriteLine("(no transactions)");
                    }
                    else
                    {
                        foreach (var tx in wallet.Transactions)
                        {
                            Console.WriteLine(tx);
                        }
                    }
                }
            }

            if (!walletExists)
            {
                Console.WriteLine("Wallet not found. Please try again.");
            }
            else
            {
                Console.WriteLine("Type Back to get list of all Wallets.");
                _appState = AppState.End;
            }
        }
    }
}