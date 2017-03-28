using PromisePayDotNet.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromisePayDotNet.Internals;
namespace PromisePayDotNet.Abstractions
{
    /// <summary>
    /// https://reference.assemblypayments.com/#users
    /// Payments can be paid and received by Users (buyers and/or sellers). Once a user is set up they can be associated with various objects, including Accounts, Items, Companies, and Addresses. There are a number of data requirements when creating Users, notably for sellers.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieve an ordered and paginated list of existing Users.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> ListUsersAsync(int limit = 10, int offset = 0, string search=null);
        /// <summary>
        /// Show details of a specific User using a given id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(string userId);
        /// <summary>
        /// Create a User. Users can be associated with Items either as a buyer or a seller.
        /// Users can’t be both the buyer and seller for the same Item.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);
        /// <summary>
        /// Show details of a specific User using a given id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> UpdateUserAsync(User user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(string userId);
        /// <summary>
        /// Retrieve an ordered and paginated list of existing Items the User is associated with using a given id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Item>> ListItemsForUserAsync(string userId);
        /// <summary>
        /// Show a User’s PayPal Account using a given :id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<PayPalAccount>> ListPayPalAccountsForUserAsync(string userId);
        /// <summary>
        /// Show the User’s Bank Account using a given :id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<CardAccount>> ListCardAccountsForUserAsync(string userId);
        /// <summary>
        /// Show the User’s Bank Account using a given :id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BankAccount>> ListBankAccountsForUserAsync(string userId);
        /// <summary>
        /// Set the User’s Disbursement Account using a given User :id and one of either a Bank Account :account_id or a PayPal Account :account_id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<DisbursementAccount> SetDisbursementAccountAsync(string userId, string accountId);
    }

    public static class UserRepositoryExtensions
    {
        public static IEnumerable<User> ListUsers(this IUserRepository repo, int limit = 10, int offset = 0)=>
            repo.ListUsersAsync(offset: offset, limit: limit).WrapResult();
        
        public static User GetUserById(this IUserRepository repo, string userId)=>
            repo.GetUserByIdAsync(userId).WrapResult();
        
        public static User CreateUser(this IUserRepository repo, User user)=>
            repo.CreateUserAsync(user).WrapResult();
        
        public static User UpdateUser(this IUserRepository repo, User user)=>
            repo.UpdateUserAsync(user).WrapResult();
        
        public static bool DeleteUser(this IUserRepository repo, string userId)=>
            repo.DeleteUserAsync(userId).WrapResult();
        
        public static IEnumerable<Item> ListItemsForUser(this IUserRepository repo, string userId)=>
            repo.ListItemsForUserAsync(userId).WrapResult();
        
        public static IEnumerable<PayPalAccount> ListPayPalAccountsForUser(this IUserRepository repo, string userId)=>
            repo.ListPayPalAccountsForUserAsync(userId).WrapResult();
        
        public static IEnumerable<CardAccount> ListCardAccountsForUser(this IUserRepository repo, string userId)=>
            repo.ListCardAccountsForUserAsync(userId).WrapResult();
        
        public static IEnumerable<BankAccount> ListBankAccountsForUser(this IUserRepository repo, string userId)=>
            repo.ListBankAccountsForUserAsync(userId).WrapResult();
        
        public static DisbursementAccount SetDisbursementAccount(this IUserRepository repo, string userId, string accountId)=>
            repo.SetDisbursementAccountAsync(accountId: accountId, userId: userId).WrapResult();
        

        public static BankAccount GetBankAccountForUser(this IUserRepository repo, string userId)=>
            repo.ListBankAccountsForUser(userId)?.FirstOrDefault();
        
        public static CardAccount GetCardAccountForUser(this IUserRepository repo, string userId)=>
            repo.ListCardAccountsForUser(userId)?.FirstOrDefault();
        
        public static PayPalAccount GetPayPalAccountForUser(this IUserRepository repo, string userId)=>
            repo.ListPayPalAccountsForUser(userId)?.FirstOrDefault();
        /// <summary>
        /// Show the User’s Bank Account using a given :id.
        /// </summary>
        public static async Task<BankAccount> GetBankAccountForUserAsync(this IUserRepository repo, string userId)
        {
            var r = await repo.ListBankAccountsForUserAsync(userId);
            return r?.FirstOrDefault();
        }
        /// <summary>
        /// Show the User’s Bank Account using a given :id.
        /// </summary>
        public static async Task<CardAccount> GetCardAccountForUserAsync(this IUserRepository repo, string userId)
        {
            var r = await repo.ListCardAccountsForUserAsync(userId);
            return r?.FirstOrDefault();
        }
        /// <summary>
        /// Show a User’s PayPal Account using a given :id.
        /// </summary>
        public static async Task<PayPalAccount> GetPayPalAccountForUserAsync(this IUserRepository repo, string userId)
        {
            var r = await repo.ListPayPalAccountsForUserAsync(userId);
            return r?.FirstOrDefault();
        }
    }
}
