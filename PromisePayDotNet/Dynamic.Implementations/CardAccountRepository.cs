using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class CardAccountRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.ICardAccountRepository
    {
        public CardAccountRepository(IRestClient client, ISettings settings, ILogger<CardAccountRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> GetCardAccountById(string cardAccountId)
        {
            AssertIdNotNull(cardAccountId);
            var request = new RestRequest("/card_accounts/{id}", Method.GET);
            request.AddUrlSegment("id", cardAccountId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> CreateCardAccount(IDictionary<string, object> cardAccount)
        {
            var request = new RestRequest("/card_accounts", Method.POST);
            request.AddParameter("user_id", (string)cardAccount["user_id"]);

            var card = (IDictionary<string, object>)(cardAccount["card"]);

            foreach (var key in card.Keys) {
                request.AddParameter(key, (string)card[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public bool DeleteCardAccount(string cardAccountId)
        {
            AssertIdNotNull(cardAccountId);
            var request = new RestRequest("/card_accounts/{id}", Method.DELETE);
            request.AddUrlSegment("id", cardAccountId);
            var response = SendRequest(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public IDictionary<string, object> GetUserForCardAccount(string cardAccountId)
        {
            AssertIdNotNull(cardAccountId);
            var request = new RestRequest("/card_accounts/{id}/users", Method.GET);
            request.AddUrlSegment("id", cardAccountId);
            IRestResponse response = SendRequest(Client, request);

            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

    }
}
