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
    public class TokenRepository : AbstractRepository, ITokenRepository
    {
        public TokenRepository(IRestClient client, ISettings settings, ILogger<TokenRepository> logger) : base(client, settings, logger)
        {
        }

        public string RequestToken()
        {
            var request = new RestRequest("/request_token", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content).Values.First();            
        }

        public IDictionary<string, object> RequestSessionToken(Token token)
        {
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
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);                     
        }
        public Widget GetWidget(string sessionToken)
        {
            var request = new RestRequest("/widget", Method.GET);
            request.AddParameter("session_token", sessionToken);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("widget"))
            {
                var itemCollection = dict["widget"];
                return JsonConvert.DeserializeObject<Widget>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }
    }
}
