using PromisePayDotNet.DTO;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromisePayDotNet.Abstractions
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieve an ordered and paginated list of Transactions.
        /// </summary>
        Task<IEnumerable<Transaction>> ListTransactionsAsync(int limit = 10, int offset = 0);
        /// <summary>
        /// Show details of a specific Transaction using a given transactionId.
        /// </summary>
        Task<Transaction> GetTransactionAsync(string transactionId);
        /// <summary>
        /// Show the User associated with the Transaction using a given :id.
        /// </summary>
        Task<User> GetUserForTransactionAsync(string transactionId);
        /// <summary>
        /// Show the Fees associated with the Transaction using a given :id.
        /// </summary>
        Task<Fee> GetFeeForTransactionAsync(string transactionId);
    }

    public static class TransactionRepositoryExtensions
    {
        public static IEnumerable<Transaction> ListTransactions(this ITransactionRepository repo, int limit = 10, int offset = 0) =>
            repo.ListTransactionsAsync(offset: offset, limit: limit).WrapResult();
        public static Transaction GetTransaction(this ITransactionRepository repo, string transactionId) =>
            repo.GetTransactionAsync(transactionId).WrapResult();
        public static User GetUserForTransaction(this ITransactionRepository repo, string transactionId) =>
            repo.GetUserForTransactionAsync(transactionId).WrapResult();
        public static Fee GetFeeForTransaction(this ITransactionRepository repo, string transactionId) =>
            repo.GetFeeForTransactionAsync(transactionId).WrapResult();
    }
}
