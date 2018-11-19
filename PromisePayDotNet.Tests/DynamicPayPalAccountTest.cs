using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicPayPalAccountTest : AbstractTest
    {
        [Test]
        public void PayPalAccountDeserialization()
        {
            var jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-25T12:31:39.324Z\", \"updated_at\": \"2015-04-25T12:31:39.324Z\", \"id\": \"70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"currency\": \"USD\", \"paypal\": { \"email\": \"test.me@promisepay.com\" }, \"links\": { \"self\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"users\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779/users\" } }";
            var payPalAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.AreEqual("70d93fe3-6c2e-4a1c-918f-13b8e7bb3779", (string)payPalAccount["id"]);
            Assert.AreEqual("USD", (string)payPalAccount["currency"]);
            var paypal = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(payPalAccount["paypal"]));
            Assert.AreEqual("test.me@promisepay.com", (string)paypal["email"]);
        }

        [Test]
        public void CreatePayPalAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new Dictionary<string, object>
            { { "user_id", userId },
                {"active" , true},
                {"paypal" , new Dictionary<string, object>
                {
                    {"paypal_email", "aaa@bbb.com"}
                }}
            };
            var resp = repo.CreatePayPalAccount(account);
            var createdAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));

            Assert.IsNotNull(createdAccount);
            Assert.IsNotNull(createdAccount["id"]);
            Assert.AreEqual("AUD", (string)createdAccount["currency"]); // It seems that currency is determined by country
            Assert.IsNotNull(createdAccount["created_at"]);
            Assert.IsNotNull(createdAccount["updated_at"]);
        }

        [Test]
        public void GetPayPalAccountSuccessfully()
        {
            var id = "cd2ab053-25e5-491a-a5ec-0c32dbe76efa";
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var resp = repo.GetPayPalAccountById(id);
            var gotAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual(id, (string)gotAccount["id"]);
        }

        [Test]
        public void GetPayPalAccountEmptyId()
        {
            var client = GetMockClient("");

            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            Assert.Throws<ArgumentException>(() => repo.GetPayPalAccountById(string.Empty));
        }

        [Test]
        public void GetUserForPayPalAccountSuccessfully()
        {
            var id = "3a780d4a-5de0-409c-9587-080930ddea3c";

            var content = File.ReadAllText("./Fixtures/paypal_account_get_users.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before

            var resp = repo.GetUserForPayPalAccount(id);

            Assert.IsNotNull(resp);
            var users = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["users"]));
            Assert.AreEqual(userId, users["id"]);
        }

        [Test]
        public void DeletePayPalAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/paypal_account_delete.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var result = repo.DeletePayPalAccount("cd2ab053-25e5-491a-a5ec-0c32dbe76efa");
            Assert.IsTrue(result);
        }

    }
}
