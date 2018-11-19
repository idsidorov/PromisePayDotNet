using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class TokenRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                    PromisePayDotNet.Dynamic.Interfaces.ITokenRepository
    {
        public TokenRepository(IRestClient client, ISettings settings, ILogger<TokenRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string,object> RequestToken()
        {
            var request = new RestRequest("/request_token", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestSessionToken(IDictionary<string,object> token)
        {
            var request = new RestRequest("/request_session_token", Method.GET);

            foreach (var key in token.Keys) {
                request.AddParameter(key, token[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string,object> GetWidget(string sessionToken)
        {
            var request = new RestRequest("/widget", Method.GET);
            request.AddParameter("session_token", sessionToken);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GenerateCardToken(string tokenType, string userId) 
        {
            var request = new RestRequest("/token_auths", Method.POST);
            request.AddParameter("token_type", tokenType);
            request.AddParameter("user_id", userId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
