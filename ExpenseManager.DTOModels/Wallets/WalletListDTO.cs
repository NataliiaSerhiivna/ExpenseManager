using ExpenseManager.Common.Enums;
using System;

namespace ExpenseManager.DTOModels.Wallets
{
    public class WalletListDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public Valuta Valuta { get; }
        public decimal TotalAmount { get; }

        public WalletListDTO(Guid id, string name, Valuta valuta, decimal totalAmount)
        {
            Id = id;
            Name = name;
            Valuta = valuta;
            TotalAmount = totalAmount;
        }
    }
}