using Newtonsoft.Json;
using Xunit;
using System.Collections.Generic;
using System.IO;
using PromisePayDotNet.Abstractions;

namespace PromisePayDotNet.Tests
{
    public class DynamicAddressTest : AbstractTest
    {
        [Fact]
        public void AddressDeserialization()
        {
            var jsonStr = "{ \"addressline1\": null, \"addressline2\": null, \"postcode\": null, \"city\": null, \"state\": null, \"id\": \"07ed45e5-bb9d-459f-bb7b-a02ecb38f443\", \"country\": \"Australia\", \"links\": { \"self\": \"/addresses/07ed45e5-bb9d-459f-bb7b-a02ecb38f443\" }}";
            var address = JsonConvert.DeserializeObject<IDictionary<string,object>>(jsonStr);
            Assert.NotNull(address);
            Assert.Equal("07ed45e5-bb9d-459f-bb7b-a02ecb38f443", (string)address["id"]);
        }

        [Fact]
        public void GetAddressSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/address_get_by_id.json");

            var client = GetMockClient(content);

            var repo = Get<IAddressRepository>(client.Object);

            var address = repo.GetAddressById("07ed45e5-bb9d-459f-bb7b-a02ecb38f443");
            client.VerifyAll();
            Assert.NotNull(address);
            Assert.Equal("07ed45e5-bb9d-459f-bb7b-a02ecb38f443", (string)address.Id);

        }
    }
}
