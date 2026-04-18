using ExpenseManager.DTOModels.Transactions;
using ExpenseManager.DTOModels.Wallets;

namespace ExpenseManager.Services
{
    public static class Validators
    {
        public record struct ValidationError(string ErrorMessage, string MemberName);

        public static List<ValidationError> Validate(this TransactionCreateDTO transactionCandidate)
        {
            var errors = new List<ValidationError>();

            if (transactionCandidate.WalletId == Guid.Empty)
            {
                errors.Add(new ValidationError(
                    "Transaction must be assigned to a wallet.",
                    nameof(TransactionCreateDTO.WalletId)));
            }

            errors.AddRange(ValidateTransaction(
                transactionCandidate.Amount,
                transactionCandidate.Category,
                transactionCandidate.Timestamp,
                transactionCandidate.Description));

            return errors;
        }

        public static List<ValidationError> Validate(this TransactionEditDTO transactionCandidate)
        {
            var errors = new List<ValidationError>();

            if (transactionCandidate.WalletId == Guid.Empty)
            {
                errors.Add(new ValidationError(
                    "Transaction must be assigned to a wallet.",
                    nameof(TransactionEditDTO.WalletId)));
            }

            errors.AddRange(ValidateTransaction(
                transactionCandidate.Amount,
                transactionCandidate.Category,
                transactionCandidate.Timestamp,
                transactionCandidate.Description));

            return errors;
        }

        public static List<ValidationError> ValidateTransaction(
            decimal? amount,
            ExpenseManager.Common.Enums.Category? category,
            DateTime? timestamp,
            string? description)
        {
            var errors = new List<ValidationError>();

            if (amount == null)
            {
                errors.Add(new ValidationError(
                    "Amount must be specified.",
                    nameof(TransactionCreateDTO.Amount)));
            }
            else if (amount == 0)
            {
                errors.Add(new ValidationError(
                    "Amount cannot be 0.",
                    nameof(TransactionCreateDTO.Amount)));
            }

            if (category == null)
            {
                errors.Add(new ValidationError(
                    "Transaction category must be selected.",
                    nameof(TransactionCreateDTO.Category)));
            }

            if (timestamp == null)
            {
                errors.Add(new ValidationError(
                    "Transaction date must be selected.",
                    nameof(TransactionCreateDTO.Timestamp)));
            }
            else if (timestamp > DateTime.Now)
            {
                errors.Add(new ValidationError(
                    "Transaction date cannot be in the future.",
                    nameof(TransactionCreateDTO.Timestamp)));
            }

            errors.AddRange(ValidateComment(
                description,
                nameof(TransactionCreateDTO.Description),
                "Comment"));

            return errors;
        }

        public static List<ValidationError> Validate(this WalletCreateDTO walletCandidate)
        {
            return ValidateWallet(
                walletCandidate.Name,
                walletCandidate.Valuta);
        }

        public static List<ValidationError> Validate(this WalletEditDTO walletCandidate)
        {
            return ValidateWallet(
                walletCandidate.Name,
                walletCandidate.Valuta);
        }

        public static List<ValidationError> ValidateWallet(
            string? name,
            ExpenseManager.Common.Enums.Valuta? valuta)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add(new ValidationError(
                    "Wallet name can't be empty.",
                    nameof(WalletCreateDTO.Name)));
            }
            else if (name.Length < 2)
            {
                errors.Add(new ValidationError(
                    "Wallet name must be at least 2 characters long.",
                    nameof(WalletCreateDTO.Name)));
            }

            if (valuta == null)
            {
                errors.Add(new ValidationError(
                    "Wallet currency must be selected.",
                    nameof(WalletCreateDTO.Valuta)));
            }

            return errors;
        }

        private static List<ValidationError> ValidateComment(string? comment, string propertyName, string displayName)
        {
            var errors = new List<ValidationError>();

            if (comment is null)
                return errors;

            if (comment.Length > 200)
            {
                errors.Add(new ValidationError(
                    $"{displayName} cannot be longer than 200 characters.",
                    propertyName));
            }

            return errors;
        }



    }
}