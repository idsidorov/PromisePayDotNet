using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using PromisePayDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicRestrictionTest : AbstractTest
    {
        [Test]
        public void ListRestrictionSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/restriction_list.json");
            var client = GetMockClient(content);

            var repo = new RestrictionRepository(client.Object, GetMockSettings().Object, GetMockLogger<RestrictionRepository>().Object);
            var response = repo.List();
            Assert.IsNotNull(response);
            var arr = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(response["payment_restrictions"]));
            Assert.AreEqual("12a7732c-87a8-432d-a814-b53c1586ec3c", arr[0]["id"]);
        }

        [Test]
        public void ShowRestrictionSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/restriction_show.json");
            var client = GetMockClient(content);
            var id = "12a7732c-87a8-432d-a814-b53c1586ec3c";
            var repo = new RestrictionRepository(client.Object, GetMockSettings().Object, GetMockLogger<RestrictionRepository>().Object);
            var response = repo.Show(id);
            Assert.IsNotNull(response);
            var restriction = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["payment_restrictions"]));
            Assert.AreEqual(id, restriction["id"]);
        }
    }
}
