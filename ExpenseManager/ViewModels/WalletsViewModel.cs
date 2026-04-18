using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Pages;
using ExpenseManager.Services;
using System.Collections.ObjectModel;

namespace ExpenseManager.ViewModels
{
    public partial class WalletsViewModel : BaseViewModel
    {
        private readonly IWalletService _walletService;
        private List<WalletListDTO> _allWallets = new List<WalletListDTO>();

        [ObservableProperty]
        public ObservableCollection<WalletListDTO> _wallets;

        [ObservableProperty]
        public WalletListDTO _selectedWallet;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private Valuta? _selectedValutaFilter;

        [ObservableProperty]
        private WalletSortOption _selectedSortOption;

        public WalletSortOption[] SortOptions => Enum.GetValues<WalletSortOption>();

        public Valuta?[] ValutaFilters =>
            new Valuta?[] { null }
            .Concat(Enum.GetValues<Valuta>().Select(valuta => (Valuta?)valuta))
            .ToArray();

        public WalletsViewModel(IWalletService walletService)
        {
            _walletService = walletService;
            Wallets = new ObservableCollection<WalletListDTO>();
            SearchText = string.Empty;
            SelectedSortOption = WalletSortOption.ByName;
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSelectedValutaFilterChanged(Valuta? value)
        {
            ApplyFilters();
        }

        partial void OnSelectedSortOptionChanged(WalletSortOption value)
        {
            ApplyFilters();
        }

        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                _allWallets = new List<WalletListDTO>();

                await foreach (var wallet in _walletService.GetAllWalletsAsync())
                {
                    _allWallets.Add(wallet);
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to load wallets: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<WalletListDTO> query = _allWallets;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(wallet =>
                    wallet.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedValutaFilter != null)
            {
                query = query.Where(wallet => wallet.Valuta == SelectedValutaFilter.Value);
            }

            query = SelectedSortOption switch
            {
                WalletSortOption.ByBalance => query.OrderByDescending(wallet => wallet.TotalAmount),
                _ => query.OrderBy(wallet => wallet.Name)
            };

            Wallets = new ObservableCollection<WalletListDTO>(query);
        }

        [RelayCommand]
        private async Task LoadWallet()
        {
            IsBusy = true;
            try
            {
                if (SelectedWallet == null)
                    return;

                await Shell.Current.GoToAsync(
                    $"{nameof(WalletDetailsPage)}",
                    new Dictionary<string, object>
                    {
                        { "WalletId", SelectedWallet.Id }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to navigate to wallet details: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddWallet()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(nameof(WalletCreatePage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate to wallet create page: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteWallet(WalletListDTO wallet)
        {
            IsBusy = true;
            try
            {
                bool confirmed = await Shell.Current.DisplayAlertAsync(
                    "Confirm",
                    "Are you sure you want to delete this wallet?",
                    "Yes",
                    "No");

                if (!confirmed)
                    return;

                await _walletService.DeleteWalletAsync(wallet.Id);

                _allWallets.RemoveAll(w => w.Id == wallet.Id);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete wallet: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EditWallet(WalletListDTO wallet)
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(
                    nameof(WalletEditPage),
                    new Dictionary<string, object>
                    {
                        { "WalletId", wallet.Id }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate to wallet edit page: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}