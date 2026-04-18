using ExpenseManager.DTOModels.Wallets;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Services
{
    public interface IWalletService
    {
        IAsyncEnumerable<WalletListDTO> GetAllWalletsAsync();
        Task<WalletDetailsDTO?> GetWalletAsync(Guid walletId);
        Task CreateWalletAsync(WalletCreateDTO walletCreateDTO);
        Task UpdateWalletAsync(WalletEditDTO walletEditDTO);
        Task DeleteWalletAsync(Guid walletId);
    }
}