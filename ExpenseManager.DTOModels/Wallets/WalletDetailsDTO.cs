using ExpenseManager.Common.Enums;
using System;

namespace ExpenseManager.DTOModels.Wallets
{
    public class WalletDetailsDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public Valuta Valuta { get; }
        public decimal TotalAmount { get; }

        public WalletDetailsDTO(Guid id, string name, Valuta valuta)
        {
            Id = id;
            Name = name;
            Valuta = valuta;
        }
    }
}