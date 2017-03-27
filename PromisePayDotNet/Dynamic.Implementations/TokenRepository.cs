using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class TokenRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                    PromisePayDotNet.Dynamic.Interfaces.ITokenRepository
    {
        public TokenRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<TokenRepository>(), options)
        {
        }


        public string RequestToken()
        {
            var request = new RestRequest("/request_token", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, string>>(response.Content).Values.First();
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
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("widget"))
            {
                var itemCollection = dict["widget"];
                return JsonConvert.DeserializeObject<IDictionary<string,object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public IDictionary<string, object> GenerateCardToken(string tokenType, string userId) 
        {
            var request = new RestRequest("/token_auths", Method.POST);
            request.AddParameter("token_type", tokenType);
            request.AddParameter("user_id", userId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("token_auth"))
            {
                var itemCollection = dict["token_auth"];
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }
    }
}
