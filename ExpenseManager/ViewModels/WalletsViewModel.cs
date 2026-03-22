using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Pages;
using ExpenseManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExpenseManager.ViewModels
{
    public class WalletsViewModel
    {
        private readonly IWalletService _walletService;

        public ObservableCollection<WalletListDTO> Wallets { get; set; }
        public WalletListDTO SelectedWallet { get; set; }
        public Command WalletSelectedCommand { get; }

        public WalletsViewModel(IWalletService walletService)
        {
            _walletService = walletService;

            Wallets = new ObservableCollection<WalletListDTO>(_walletService.GetAllWallets());
            WalletSelectedCommand = new Command(LoadWallet);
        }

        private void LoadWallet()
        {
            if (SelectedWallet == null)
                return;

            Shell.Current.GoToAsync(
                $"{nameof(WalletDetailsPage)}",
                new Dictionary<string, object> { { "WalletId", SelectedWallet.Id } });
        }
    }
}