using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class RestrictionRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IRestrictionRepository
    {
        public RestrictionRepository(IRestClient client, ISettings settings, ILogger<RestrictionRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> List()
        {
            var request = new RestRequest("/payment_restrictions", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> Show(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/payment_restrictions/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
