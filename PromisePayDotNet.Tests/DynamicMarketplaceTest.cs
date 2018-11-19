using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class DynamicMarketplaceTest : AbstractTest
    {
        [Test]
        public void ShowSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/marketplaces_show.json");
            var client = GetMockClient(content);

            var repo = new MarketplaceRepository(client.Object, GetMockSettings().Object, GetMockLogger<MarketplaceRepository>().Object);
            var response = repo.ShowMarketplace();
            client.VerifyAll();

            Assert.IsNotNull(response);
            var marketplace = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["marketplaces"]));
            Assert.AreEqual("c298d7ea-ea8c-486e-95d9-1e45ae3787ab", marketplace["id"]);
        }
    }
}
