using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using System.Linq;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class TransactionRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.ITransactionRepository
    {
        public TransactionRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<TransactionRepository>(), options)
        {
        }


        public IEnumerable<IDictionary<string,object>> ListTransactions(int limit = 10, int offset = 0)
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
                return JsonConvert.DeserializeObject<List<IDictionary<string, object>>>(JsonConvert.SerializeObject(transactionCollection));
            }
            return new List<IDictionary<string, object>>();
        }

        public IDictionary<string, object> GetTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            var result =  JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(result));
        }

        public IDictionary<string, object> GetUserForTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}/users", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var itemCollection = dict["users"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public IDictionary<string, object> GetFeeForTransaction(string transactionId)
        {
            AssertIdNotNull(transactionId);
            var request = new RestRequest("/transactions/{id}/fees", Method.GET);
            request.AddUrlSegment("id", transactionId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("fees"))
            {
                var itemCollection = dict["fees"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

    }
}
