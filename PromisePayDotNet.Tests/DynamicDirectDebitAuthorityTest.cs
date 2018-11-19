using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicDirectDebitAuthorityTest : AbstractTest
    {
        [Test]
        public void CreateSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/direct_debit_authorities_create.json");

            var client = GetMockClient(content);
            var repo = new DirectDebitAuthorityRepository(client.Object, GetMockSettings().Object, GetMockLogger<DirectDebitAuthorityRepository>().Object);
            var resp = repo.Create("9fda18e7-b1d3-4a83-830d-0cef0f62cd25", "100000");
            client.VerifyAll();
            Assert.IsNotNull(resp);
            var id = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["direct_debit_authorities"]))["id"];
            Assert.AreEqual("8f233e04-ffaa-4c9d-adf9-244853848e21",id);
        }

        [Test]
        public void ListSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/direct_debit_authorities_list.json");

            var client = GetMockClient(content);
            var repo = new DirectDebitAuthorityRepository(client.Object, GetMockSettings().Object, GetMockLogger<DirectDebitAuthorityRepository>().Object);
            var resp = repo.List("9fda18e7-b1d3-4a83-830d-0cef0f62cd25");
            client.VerifyAll();
            Assert.IsNotNull(resp);
            var first = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(resp["direct_debit_authorities"])).First();
            Assert.AreEqual("8f233e04-ffaa-4c9d-adf9-244853848e21", first["id"]);
        }

        [Test]
        public void ShowSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/direct_debit_authorities_show.json");

            var client = GetMockClient(content);
            var repo = new DirectDebitAuthorityRepository(client.Object, GetMockSettings().Object, GetMockLogger<DirectDebitAuthorityRepository>().Object);
            var resp = repo.Show("8f233e04-ffaa-4c9d-adf9-244853848e21");
            client.VerifyAll();
            Assert.IsNotNull(resp);
            var id = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["direct_debit_authorities"]))["id"];
            Assert.AreEqual("8f233e04-ffaa-4c9d-adf9-244853848e21", id);
        }

        [Test]
        public void DeleteSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/direct_debit_authorities_delete.json");

            var client = GetMockClient(content);
            var repo = new DirectDebitAuthorityRepository(client.Object, GetMockSettings().Object, GetMockLogger<DirectDebitAuthorityRepository>().Object);
            var resp = repo.Delete("9fda18e7-b1d3-4a83-830d-0cef0f62cd25");
            client.VerifyAll();
            Assert.IsNull(resp);
        }

    }
}
