using Newtonsoft.Json;
using PromisePayDotNet.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class ConfigurationRepository : PromisePayDotNet.Implementations.AbstractRepository,
        PromisePayDotNet.Dynamic.Interfaces.IConfigurationRepository
    {
        public ConfigurationRepository(IRestClient client, ISettings settings, ILogger<ConfigurationRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string, object> Create(IDictionary<string, object> configuration)
        {
            var request = new RestRequest("/configurations", Method.POST);

            if (!configuration.ContainsKey("name")) 
            {
                throw new ValidationException("configuration should contain \"name\" field");
            }

            foreach (var key in configuration.Keys)
            {
                request.AddParameter(key, (string)configuration[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> List()
        {
            var request = new RestRequest("/configurations", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> Show(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/configurations/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }


        public IDictionary<string, object> Update(IDictionary<string, object> configuration)
        {
            var request = new RestRequest("/configurations/{id}", Method.PATCH);

            if (!configuration.ContainsKey("name"))
            {
                throw new ValidationException("configuration should contain \"name\" field");
            }

            if (!configuration.ContainsKey("id"))
            {
                throw new ValidationException("configuration should contain \"id\" field");
            }

            foreach (var key in configuration.Keys)
            {
                request.AddParameter(key, (string)configuration[key]);
            }

            request.AddUrlSegment("id", (string)configuration["id"]);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);            
        }

        public IDictionary<string, object> Delete(string id)
        {
            AssertIdNotNull(id);
            var request = new RestRequest("/configurations/{id}", Method.DELETE);
            request.AddUrlSegment("id", id);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);            
        }
    }
}
