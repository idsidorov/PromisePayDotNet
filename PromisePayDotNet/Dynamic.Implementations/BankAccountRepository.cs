using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class BankAccountRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IBankAccountRepository
    {
        public BankAccountRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<BankAccountRepository>(), options)
        {
        }


        public IDictionary<string, object> GetBankAccountById(string bankAccountId)
        {
            AssertIdNotNull(bankAccountId);
            var request = new RestRequest("/bank_accounts/{id}", Method.GET);
            request.AddUrlSegment("id", bankAccountId);
            var response = SendRequest(Client, request);
            var first = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            var firstJson = JsonConvert.SerializeObject(first);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(firstJson);
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
            return JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string,object>>>(response.Content).Values.First();
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
            RestResponse response = SendRequest(Client, request);

            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var item = dict["users"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(item));
            }
            return null;
        }
    }

}
