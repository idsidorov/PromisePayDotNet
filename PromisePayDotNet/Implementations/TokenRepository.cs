using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace PromisePayDotNet.Implementations
{
    internal class TokenRepository : AbstractRepository, ITokenRepository
    {
        public TokenRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<TokenRepository>(), options)
        {
        }


        public async Task<string> RequestTokenAsync()
        {
            // NOTE: there is no doc related to this!
            var request = new RestRequest("/request_token", Method.GET);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content).Values.First();
        }

        public async Task<IDictionary<string, object>> RequestSessionTokenAsync(Token token)
        {
            // NOTE: there is no doc related to this!
            var request = new RestRequest("/request_session_token", Method.GET);
            request.AddParameter("current_user_id", token.CurrentUserId);
            request.AddParameter("current_user", token.CurrentUser);
            request.AddParameter("item_name", token.ItemName);
            request.AddParameter("amount", token.Amount);
            request.AddParameter("seller_lastname", token.SellerLastName);
            request.AddParameter("seller_firstname", token.SellerFirstName);
            request.AddParameter("seller_country", token.SellerCountry);
            request.AddParameter("buyer_lastname", token.BuyerLastName);
            request.AddParameter("buyer_firstname", token.BuyerFirstName);
            request.AddParameter("buyer_country", token.BuyerCountry);
            request.AddParameter("seller_email", token.SellerEmail);
            request.AddParameter("buyer_email", token.BuyerEmail);
            request.AddParameter("external_item_id", token.ExternalItemId);
            request.AddParameter("external_seller_id", token.ExternalSellerId);
            request.AddParameter("external_buyer_id", token.ExternalBuyerId);
            request.AddParameter("fee_ids", token.FeeIds);
            request.AddParameter("payment_type_id", (int)token.PaymentType);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public async Task<Widget> GetWidgetAsync(string sessionToken)
        {
            // NOTE: there is no doc related to this!
            var request = new RestRequest("/widget", Method.GET);
            request.AddParameter("session_token", sessionToken);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("widget"))
            {
                var itemCollection = dict["widget"];
                return JsonConvert.DeserializeObject<Widget>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public async Task<CardToken> GenerateCardTokenAsync(string tokenType, string userId)
        {
            var request = new RestRequest("/token_auths", Method.POST);
            request.AddParameter("token_type", tokenType);
            request.AddParameter("user_id", userId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("token_auth"))
            {
                var itemCollection = dict["token_auth"];
                return JsonConvert.DeserializeObject<CardToken>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }
    }
}
