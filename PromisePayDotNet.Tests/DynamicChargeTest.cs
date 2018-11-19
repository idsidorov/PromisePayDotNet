using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicChargeTest : AbstractTest
    {
        [Test]
        public void ListChargesSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/charges_list.json");
            var client = GetMockClient(content);

            var repo = new ChargeRepository(client.Object, GetMockSettings().Object, GetMockLogger<ChargeRepository>().Object);
            var charges = repo.ListCharges();
            client.VerifyAll();

            Assert.IsNotNull(charges);
        }

        [Test]
        public void CreateChargeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/charges_create.json");
            var client = GetMockClient(content);

            var repo = new ChargeRepository(client.Object, GetMockSettings().Object, GetMockLogger<ChargeRepository>().Object);
            var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";
            var charge = new Dictionary<string, object>
            {
                {"name" , "Charge for Delivery"},
                {"account_id" , "b49d943f-add0-4d1c-b357-0f1a8fde677c"},
                {"amount" , "4500"},
                {"email" , "abc@abc.com"},
                {"zip" , "3000"},
                {"country" , "AUS"},
                {"user_id" , "7af96d61-2339-4298-8a09-aadd6c4501b2"},
                {"fee_ids" , "187"},
                {"currency" , "AUD"},
                {"retain_account" , "false"},
                {"device_id" , "123456"},
                {"ip_address" , "127.0.0.1"}
            };

            var response = repo.CreateCharge(charge);

            var createdCharge = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["charges"]));

            Assert.AreEqual(id, createdCharge["id"]);
            Assert.AreEqual(charge["name"], createdCharge["name"]);
            Assert.IsTrue(((DateTime?)createdCharge["created_at"]).HasValue);
            Assert.IsTrue(((DateTime?)createdCharge["updated_at"]).HasValue);

        }

        [Test]
        public void ShowChargeSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/charges_show.json");
            var client = GetMockClient(content);
            var repo = new ChargeRepository(client.Object, GetMockSettings().Object, GetMockLogger<ChargeRepository>().Object);
            var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";

            var response = repo.ShowCharge(id);
            var charge = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["charges"]));

            Assert.IsNotNull(charge);
            Assert.AreEqual(id, charge["id"]);
        }

        [Test]
        public void ShowChargeBuyerSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/charges_show_buyer.json");
            var client = GetMockClient(content);
            var repo = new ChargeRepository(client.Object, GetMockSettings().Object, GetMockLogger<ChargeRepository>().Object);
            var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";
            var buyerId = "1be7f54f-c09f-4298-a665-f3a9f1dac60c";

            var response = repo.ShowChargeBuyer(id);
            var buyer = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["users"]));

            Assert.IsNotNull(buyer);
            Assert.AreEqual(buyerId, buyer["id"]);
        }

        [Test]
        public void ShowChargeBuyerStatus()
        {
            var content = File.ReadAllText("./Fixtures/charges_show_status.json");
            var client = GetMockClient(content);
            var repo = new ChargeRepository(client.Object, GetMockSettings().Object, GetMockLogger<ChargeRepository>().Object);
            var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";

            var response = repo.ShowChargeStatus(id);
            var charge = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["charges"]));

            Assert.IsNotNull(charge);
            Assert.AreEqual(id, charge["id"]);
            Assert.AreEqual("completed", charge["state"]);
        
        }
    }
}
