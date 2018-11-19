using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicToolTest : AbstractTest
    {
        [Test]
        public void HealthCheckSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/tool_health_status.json");
            var client = GetMockClient(content);
            var repo = new ToolRepository(client.Object, GetMockSettings().Object, GetMockLogger<ToolRepository>().Object);
            var response = repo.HealthCheck();
            client.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual("healthy", response["status"]);

        }

    }
}
