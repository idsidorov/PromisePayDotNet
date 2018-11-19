using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicAddressTest : AbstractTest
    {
        [Test]
        public void AddressDeserialization()
        {
            var jsonStr = "{ \"addressline1\": null, \"addressline2\": null, \"postcode\": null, \"city\": null, \"state\": null, \"id\": \"07ed45e5-bb9d-459f-bb7b-a02ecb38f443\", \"country\": \"Australia\", \"links\": { \"self\": \"/addresses/07ed45e5-bb9d-459f-bb7b-a02ecb38f443\" }}";
            var address = JsonConvert.DeserializeObject<IDictionary<string,object>>(jsonStr);
            Assert.IsNotNull(address);
            Assert.AreEqual("07ed45e5-bb9d-459f-bb7b-a02ecb38f443", (string)address["id"]);
        }

        [Test]
        public void GetAddressSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/address_get_by_id.json");

            var client = GetMockClient(content);

            var repo = new AddressRepository(client.Object, GetMockSettings().Object, GetMockLogger<AddressRepository>().Object);

            var resp = repo.GetAddressById("07ed45e5-bb9d-459f-bb7b-a02ecb38f443");
            client.VerifyAll();
            Assert.IsNotNull(resp);
            var address = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual("07ed45e5-bb9d-459f-bb7b-a02ecb38f443", (string)address["id"]);

        }
    }
}
