using Newtonsoft.Json;
using PromisePayDotNet.Exceptions;
using RestSharp;
using System.Collections.Generic;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class ChargeRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IChargeRepository
    {
        public ChargeRepository(IRestClient client, ISettings settings, ILogger<ChargeRepository> logger) : base(client, settings, logger)
        {
        }

        #region public methods

        public IDictionary<string, object> CreateCharge(IDictionary<string, object> charge) 
        {
            var request = new RestRequest("/charges", Method.POST);

            foreach (var key in charge.Keys)
            {
                request.AddParameter(key, (string)charge[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string,object>>(response.Content);
        }

        public IDictionary<string, object> ListCharges(int limit = 10, int offset = 0)
        {
            AssertListParamsCorrect(limit, offset);
            var request = new RestRequest("/charges", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ShowCharge(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/charges/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ShowChargeBuyer(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/charges/{id}/buyer", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ShowChargeStatus(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/charges/{id}/status", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
        #endregion

        #region private methods
        private void ValidateCharge(IDictionary<string,object> charge) 
        {
            if ((!charge.ContainsKey("country")) || !IsCorrectCountryCode((string)charge["country"]))
            {
                throw new ValidationException("Field charge.Country should contain 3-letter ISO country code!");
            }
            if ((!charge.ContainsKey("email")) || !IsCorrectEmail((string)charge["email"]))
            {
                throw new ValidationException("Field charge.Email should contain correct email address!");
            }
        }
        #endregion
    }
}
