using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class DirectDebitAuthorityRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                                  PromisePayDotNet.Dynamic.Interfaces.IDirectDebitAuthorityRepository
    {
        public DirectDebitAuthorityRepository(IRestClient client, ISettings settings, ILogger<DirectDebitAuthorityRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> Create(string accountId, string amount) 
        {
            var request = new RestRequest("/direct_debit_authorities", Method.POST);

            request.AddParameter("account_id", accountId);
            request.AddParameter("amount", amount);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string,object>>(response.Content);            
        }

        public IDictionary<string, object> List(string accountId, int limit=10, int offset=0)
        {
            var request = new RestRequest("/direct_debit_authorities", Method.GET);

            request.AddParameter("account_id", accountId);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);            
        }

        public IDictionary<string, object> Show(string id)
        {
            var request = new RestRequest("/direct_debit_authorities/{id}", Method.GET);

            request.AddUrlSegment("id", id);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);            

        }

        public IDictionary<string, object> Delete(string id)
        {
            var request = new RestRequest("/direct_debit_authorities/{id}", Method.GET);

            request.AddUrlSegment("id", id);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);  
        }
    }
}
