using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class AddressRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                     PromisePayDotNet.Dynamic.Interfaces.IAddressRepository
    {
        public AddressRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<AddressRepository>(), options)
        {
        }


        public IDictionary<string,object> GetAddressById(string addressId)
        {
            AssertIdNotNull(addressId);
            var request = new RestRequest("/addresses/{id}", Method.GET);
            request.AddUrlSegment("id", addressId);
            var response = SendRequest(Client, request);
            var address = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(address));
        }
    }
}
