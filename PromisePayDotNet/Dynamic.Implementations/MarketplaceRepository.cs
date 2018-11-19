using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class MarketplaceRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IMarketplaceRepository
    {
        public MarketplaceRepository(IRestClient client, ISettings settings, ILogger<MarketplaceRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> ShowMarketplace() 
        {
            var request = new RestRequest("/marketplace", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
