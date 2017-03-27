using PromisePayDotNet.DTO;
using System.Collections.Generic;
using System.Linq;

namespace PromisePayDotNet.Abstractions
{
    public interface IUserRepository
    {
        IEnumerable<User> ListUsers(int limit = 10, int offset = 0);

        User GetUserById(string userId);

        User CreateUser(User user);

        User UpdateUser(User user);

        bool DeleteUser(string userId);

        IEnumerable<Item> ListItemsForUser(string userId);

        IEnumerable<PayPalAccount> ListPayPalAccountsForUser(string userId);

        IEnumerable<CardAccount> ListCardAccountsForUser(string userId);

        IEnumerable<BankAccount> ListBankAccountsForUser(string userId);

        DisbursementAccount SetDisbursementAccount(string userId, string accountId);

    }

    public static class UserRepositoryExtensions
    {
        public static BankAccount GetBankAccountForUser(this IUserRepository repo, string userId)
        {
            return repo.ListBankAccountsForUser(userId)?.FirstOrDefault();
        }
        public static CardAccount GetCardAccountForUser(this IUserRepository repo, string userId)
        {
            return repo.ListCardAccountsForUser(userId)?.FirstOrDefault();
        }
        public static PayPalAccount GetPayPalAccountForUser(this IUserRepository repo, string userId)
        {
            return repo.ListPayPalAccountsForUser(userId)?.FirstOrDefault();
        }
    }
}
