using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using PromisePayDotNet.Settings; 
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class ToolRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                  PromisePayDotNet.Dynamic.Interfaces.IToolRepository
    {
        public ToolRepository(IRestClient client, ISettings settings, ILogger<ToolRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> HealthCheck() 
        {
            var request = new RestRequest("/status", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
