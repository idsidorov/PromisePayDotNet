using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromisePayDotNet.Implementations
{
    internal class AddressRepository : AbstractRepository, IAddressRepository
    {
        public AddressRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<AddressRepository>(), options)
        {
        }

        public async Task<Address> GetAddressByIdAsync(string addressId)
        {
            AssertIdNotNull(addressId);
            var request = new RestRequest("/addresses/{id}", Method.GET);
            request.AddUrlSegment("id", addressId);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Address>>(response.Content).Values.First(); 
        }
    }
}
