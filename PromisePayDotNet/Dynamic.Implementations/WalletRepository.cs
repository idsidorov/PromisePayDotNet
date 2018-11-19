using Newtonsoft.Json;
using PromisePayDotNet.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class WalletRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                    PromisePayDotNet.Dynamic.Interfaces.IWalletRepository
    {
        public WalletRepository(IRestClient client, ISettings settings, ILogger<WalletRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> ShowWalletAccount(string id)
        {
            var request = new RestRequest("/wallet_accounts/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> WithdrawFunds(string id)
        {
            var request = new RestRequest("/wallet_accounts/{id}/withdraw", Method.POST);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> DepositFunds(string id)
        {
            var request = new RestRequest("/wallet_accounts/{id}/deposit", Method.POST);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ShowWalletAccountUser(string id)
        {
            var request = new RestRequest("/wallet_accounts/{id}/users", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
