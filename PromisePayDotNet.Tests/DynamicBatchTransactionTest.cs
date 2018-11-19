using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicBatchTransactionTest : AbstractTest
    {
        [Test]
        public void ListTest() 
        {
            var content = File.ReadAllText("./Fixtures/batch_list.json");

            var client = GetMockClient(content);
            var repo = new BatchTransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<BatchTransactionRepository>().Object);

            var response = repo.List();
            client.VerifyAll();
            var transaction = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(response["batch_transactions"]));

            Assert.AreEqual("4098c6fd-ca04-4e0d-9454-87def4523a23", transaction.First()["id"]);
        }

        [Test]
        public void ShowTest()
        {
            var content = File.ReadAllText("./Fixtures/batch_show.json");

            var client = GetMockClient(content);
            var repo = new BatchTransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<BatchTransactionRepository>().Object);
            const string id = "b1652611-9544-4244-a601-54c24cfa5e90";
            var response = repo.Show(id);
            client.VerifyAll();
            var transaction = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["batch_transactions"]));
            Assert.AreEqual(id, transaction["id"]);
        }
    }
}
