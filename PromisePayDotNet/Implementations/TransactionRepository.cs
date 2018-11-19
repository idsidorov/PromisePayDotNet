using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Interfaces;
using RestSharp;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Implementations
{
    public class TransactionRepository : AbstractRepository, ITransactionRepository
    {
        public TransactionRepository(IRestClient client, ISettings settings, ILogger<TransactionRepository> logger) : base(client, settings, logger)
        {
        }

        public IEnumerable<Transaction> ListTransactions(int limit = 10, int offset = 0)
        {
            AssertListParamsCorrect(limit, offset);

            var request = new RestRequest("/transactions", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("transactions"))
            {
                var transactionCollection = dict["transactions"];
                return JsonConvert.DeserializeObject<List<Transaction>>(JsonConvert.SerializeObject(transactionCollection));
            }
            return new List<Transaction>();
        }

        public Transaction GetTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Transaction>>(response.Content).Values.First();
        }

        public User GetUserForTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}/users", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var itemCollection = dict["users"];
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public Fee GetFeeForTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}/fees", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("fees"))
            {
                var itemCollection = dict["fees"];
                return JsonConvert.DeserializeObject<Fee>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }
    }
}
