using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Repositories;
using ExpenseManager.Services;
using ExpenseManager.Storage;

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
        private static IWalletService _walletService;
        private static ITransactionService _transactionService;
        private static List<WalletListDTO> _wallets;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello and welcome to the Expense Manager Console App!");

            var storageContext = new InMemoryStorageContext();
            var walletRepository = new WalletRepository(storageContext);
            var transactionRepository = new TransactionRepository(storageContext);

            _walletService = new WalletService(walletRepository, transactionRepository);
            _transactionService = new TransactionService(transactionRepository);

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
            Console.WriteLine("Here is the list of all Wallets:");
            LoadWallets();

            foreach (var wallet in _wallets)
            {
                Console.WriteLine(wallet);
            }

            Console.WriteLine("Type the name of Wallet to see its Transactions.");
        }

        private static void LoadWallets()
        {
            if (_wallets != null)
                return;

            _wallets = new List<WalletListDTO>();
            foreach (var wallet in _walletService.GetAllWallets())
            {
                _wallets.Add(wallet);
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
                    Console.WriteLine($"Transactions in {wallet.Name}:");

                    foreach (var transaction in _transactionService.GetTransactionsByWalletId(wallet.Id))
                    {
                        Console.WriteLine(transaction);
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