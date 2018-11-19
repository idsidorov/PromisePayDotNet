using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class BankAccountRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IBankAccountRepository
    {
        public BankAccountRepository(IRestClient client, ISettings settings, ILogger<BankAccountRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> GetBankAccountById(string bankAccountId)
        {
            AssertIdNotNull(bankAccountId);
            var request = new RestRequest("/bank_accounts/{id}", Method.GET);
            request.AddUrlSegment("id", bankAccountId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> CreateBankAccount(IDictionary<string, object> bankAccount)
        {
            var request = new RestRequest("/bank_accounts", Method.POST);
            request.AddParameter("user_id", (string)bankAccount["user_id"]);
            var bank = (IDictionary<string,object>)(bankAccount["bank"]);

            foreach (var key in bank.Keys) {
                request.AddParameter(key, (string)bank[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public bool DeleteBankAccount(string bankAccountId)
        {
            AssertIdNotNull(bankAccountId);
            var request = new RestRequest("/bank_accounts/{id}", Method.DELETE);
            request.AddUrlSegment("id", bankAccountId);
            var response = SendRequest(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public IDictionary<string, object> GetUserForBankAccount(string bankAccountId)
        {
            AssertIdNotNull(bankAccountId);
            var request = new RestRequest("/bank_accounts/{id}/users", Method.GET);
            request.AddUrlSegment("id", bankAccountId);
            var response = SendRequest(Client, request);

            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ValidateRoutingNumber(string routingNumber) 
        {
            AssertIdNotNull(routingNumber);
            var request = new RestRequest("/status", Method.GET);
            request.AddParameter("routing_number", routingNumber);
            var response = SendRequest(Client, request);

            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }

}
