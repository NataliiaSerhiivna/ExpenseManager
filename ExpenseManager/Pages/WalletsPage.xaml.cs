using ExpenseManager.Services;
using ExpenseManager.UIModels;
using System.Collections.ObjectModel;


namespace ExpenseManager.Pages;

public partial class WalletsPage : ContentPage
{
    private readonly IStorageService _storage;

    public ObservableCollection<WalletUIModel> Wallets { get; set; }
    public WalletsPage(IStorageService storageService)
    {
        InitializeComponent();
        _storage = storageService;
        Wallets = new ObservableCollection<WalletUIModel>();
        foreach (var Wallet in _storage.GetAllWallets())
        {
            Wallets.Add(new WalletUIModel(_storage, Wallet));
        }
        BindingContext = this;
    }

    private async void WalletSelected(object sender, SelectionChangedEventArgs e)
    {
        var wallet = (WalletUIModel)e.CurrentSelection[0];
        await Shell.Current.GoToAsync($"{nameof(WalletDetailsPage)}", new Dictionary<string, object> { { nameof(WalletDetailsPage.CurrentWallet), wallet } });
    }
}
