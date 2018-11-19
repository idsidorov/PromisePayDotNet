using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicCardAccountTest : AbstractTest
    {
        [Test]
        public void CardAccountDeserialization()
        {
            const string jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-26T06:28:55.559Z\", \"updated_at\": \"2015-04-26T06:28:55.559Z\", \"id\": \"ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"currency\": \"USD\", \"card\": { \"type\": \"visa\", \"full_name\": \"Joe Frio\", \"number\": \"XXXX-XXXX-XXXX-1111\", \"expiry_month\": \"5\", \"expiry_year\": \"2016\" }, \"links\": { \"self\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"users\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19/users\" } }";
            var cardAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.AreEqual("ea464d25-fc9a-4887-861a-3d8ec2e12c19", (string)cardAccount["id"]);
            Assert.AreEqual("USD", (string)cardAccount["currency"]);
            var card = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(cardAccount["card"]));
            
            Assert.AreEqual("Joe Frio", card["full_name"]);
        }

        [Test]
        public void CreateCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_create.json");

            var client = GetMockClient(content);
            var repo = new CardAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<CardAccountRepository>().Object);

            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new Dictionary<string, object>
            { 
                { "user_id", userId },
                { "active", true },
                { "card" , new Dictionary<string, object>
                { 
                    { "full_name", "Batman" },
                    { "expiry_month", "11" },
                    { "expiry_year", "2020" },
                    { "number" , "4111111111111111"},
                    { "type" , "visa"},
                    { "cvv" , "123"}
                }
            }};
            var resp = repo.CreateCardAccount(account);
            client.VerifyAll();
            var result = resp.Values.First();
            var createdAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(result));

            Assert.IsNotNull(createdAccount);
            Assert.IsNotNull(createdAccount["id"]);
            Assert.AreEqual("AUD", (string)createdAccount["currency"]); // It seems that currency is determined by country
            Assert.IsNotNull(createdAccount["created_at"]);
            Assert.IsNotNull(createdAccount["updated_at"]);
        }

        [Test]
        public void GetCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_get_by_id.json");

            var client = GetMockClient(content);
            var repo = new CardAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<CardAccountRepository>().Object);
            var resp = repo.GetCardAccountById("25d34744-8ef0-46a4-8b18-2a8322933cd1");
            client.VerifyAll();
            var result = resp.Values.First();
            var gotAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(result));

            Assert.AreEqual("25d34744-8ef0-46a4-8b18-2a8322933cd1", gotAccount["id"]);
        }
        
        [Test]
        public void GetCardAccountEmptyId()
        {
            var client = GetMockClient("");
            var repo = new CardAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<CardAccountRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.GetCardAccountById(string.Empty));
        }
        
        [Test]
        public void GetUserForCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_get_users.json");

            var client = GetMockClient(content);
            var repo = new CardAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<CardAccountRepository>().Object);
            var resp = repo.GetUserForCardAccount("25d34744-8ef0-46a4-8b18-2a8322933cd1");

            client.VerifyAll();

            Assert.IsNotNull(resp);

            var users = resp["users"];
            Assert.IsNotNull(users);
            var user = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(users));
            Assert.AreEqual("1", user["id"]);
        }
        
        [Test]
        public void DeleteCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_delete.json");

            var client = GetMockClient(content);
            var repo = new CardAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<CardAccountRepository>().Object);
            const string id = "25d34744-8ef0-46a4-8b18-2a8322933cd1";

            var result = repo.DeleteCardAccount(id);
            client.VerifyAll();
            Assert.IsTrue(result);
        }
        
    }
}
