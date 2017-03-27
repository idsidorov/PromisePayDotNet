using PromisePayDotNet.DTO;
using System.Collections.Generic;

namespace PromisePayDotNet.Abstractions
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> ListTransactions(int limit = 10, int offset = 0);

        Transaction GetTransaction(string transactionId);
        
        User GetUserForTransaction(string transactionId);
        
        Fee GetFeeForTransaction(string transactionId);
    }
}
