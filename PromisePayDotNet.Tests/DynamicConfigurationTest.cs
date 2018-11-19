using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using PromisePayDotNet.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class DynamicConfigurationTest : AbstractTest
    {
        [Test]
        public void CreateConfigurationSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/configuration_create.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var configuration = new Dictionary<string, object>
            {
                {"name" , "email_by_promisepay"}
            };

            var resp = repo.Create(configuration);
            var created = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["feature_configurations"]));

            Assert.AreEqual("ca321b3f-db87-4d75-ba05-531c7f1bb515", created["id"]);
            Assert.AreEqual(configuration["name"], created["name"]);
        }

        [Test]
        public void CreateConfigurationMissingName() 
        {
            var content = File.ReadAllText("./Fixtures/configuration_create.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var configuration = new Dictionary<string, object>
            {

            };
            Assert.Throws<ValidationException>(() => repo.Create(configuration));        
        }       

        [Test]
        public void ListConfigurationSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/configuration_list.json");
            var client = GetMockClient(content);

            var repo = new RestrictionRepository(client.Object, GetMockSettings().Object, GetMockLogger<RestrictionRepository>().Object);
            var response = repo.List();
            Assert.IsNotNull(response);
            var arr = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(response["feature_configurations"]));
            Assert.AreEqual("ca321b3f-db87-4d75-ba05-531c7f1bb515", arr[0]["id"]);
        }

        [Test]
        public void ShowConfigurationSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/configuration_show.json");
            var client = GetMockClient(content);
            var id = "ca321b3f-db87-4d75-ba05-531c7f1bb515";
            var repo = new RestrictionRepository(client.Object, GetMockSettings().Object, GetMockLogger<RestrictionRepository>().Object);
            var response = repo.Show(id);
            Assert.IsNotNull(response);
            var configuration = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["feature_configurations"]));
            Assert.AreEqual(id, configuration["id"]);
        }

        [Test]
        public void UpdateConfigurationSuccessfull()
        {
            var content = File.ReadAllText("./Fixtures/configuration_update.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var configuration = new Dictionary<string, object>
            {
                {"id" , "ca321b3f-db87-4d75-ba05-531c7f1bb515"},
                {"name" , "email_by_promisepay"}
            };

            var resp = repo.Update(configuration);
            var updated = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["feature_configurations"]));

            Assert.AreEqual(configuration["id"], updated["id"]);
            Assert.AreEqual(configuration["name"], updated["name"]);
        }

        [Test]
        public void UpdateConfigurationMissingName()
        {
            var content = File.ReadAllText("./Fixtures/configuration_update.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var configuration = new Dictionary<string, object>
            {
                {"id","someid"}
            };
            Assert.Throws<ValidationException>(() => repo.Update(configuration));
        }

        [Test]
        public void UpdateConfigurationMissingId()
        {
            var content = File.ReadAllText("./Fixtures/configuration_update.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var configuration = new Dictionary<string, object>
            {
                {"name","some name"}
            };
            Assert.Throws<ValidationException>(() => repo.Update(configuration));
        }       

        [Test]
        public void DeleteConfigurationSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/configuration_delete.json");
            var client = GetMockClient(content);
            var repo = new ConfigurationRepository(client.Object, GetMockSettings().Object, GetMockLogger<ConfigurationRepository>().Object);
            var id = "ca321b3f-db87-4d75-ba05-531c7f1bb515";
            var response = repo.Delete(id);
            var configuration = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["feature_configurations"]));
            Assert.AreEqual(id, configuration["id"]);
        }
    }
}
