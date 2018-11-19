using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicBankAccountTest : AbstractTest
    {
        [Test]
        public void BankAccountDeserialization()
        {
            const string jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-26T06:24:19.248Z\", \"updated_at\": \"2015-04-26T06:24:19.248Z\", \"id\": \"8d65c86c-14f4-4abf-a979-eba0a87b283a\", \"currency\": \"USD\", \"bank\": { \"bank_name\": \"Test Me\", \"country\": \"AUS\", \"account_name\": \"Test Account\", \"routing_number\": \"XXXXXXX3\", \"account_number\": \"XXXX344\", \"holder_type\": \"personal\", \"account_type\": \"savings\" }, \"links\": { \"self\": \"/bank_accounts/8d65c86c-14f4-4abf-a979-eba0a87b283a\", \"users\": \"/bank_accounts/8d65c86c-14f4-4abf-a979-eba0a87b283a/users\" } }";
            var bankAccount = JsonConvert.DeserializeObject<IDictionary<string,object>>(jsonStr);
            Assert.AreEqual("8d65c86c-14f4-4abf-a979-eba0a87b283a", (string)bankAccount["id"]);
            Assert.AreEqual("USD", (string)bankAccount["currency"]);
        }
        
        [Test]
        public void CreateBankAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/bank_account_create.json");

            var client = GetMockClient(content);
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);

            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new Dictionary<string, object>
            {   { "user_id", userId },
                { "active" , true },
                { "bank" , new Dictionary<string, object>
                {   {"bank_name" , "Test bank, inc"},
                    {"account_name" , "Test account"},
                    {"account_number" , "8123456789"},
                    {"account_type" , "savings"},
                    {"country" , "AUS"},
                    {"holder_type" , "personal"},
                    {"routing_number" , "123456"}
                }}};
            var response = repo.CreateBankAccount(account);
            client.VerifyAll();
            var createdAccount = JsonConvert.DeserializeObject<IDictionary<string,object>>(JsonConvert.SerializeObject(response.Values.First()));
            Assert.IsNotNull(createdAccount);
            Assert.IsNotNull(createdAccount["id"]);
            Assert.AreEqual("AUD", (string)createdAccount["currency"]); // It seems that currency is determined by country
            Assert.IsNotNull(createdAccount["created_at"]);
            Assert.IsNotNull(createdAccount["updated_at"]);
            var bank = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(createdAccount["bank"]));
            Assert.AreEqual("XXX789", (string)bank["account_number"]); //Account number is masked
        }
        
        [Test]
        public void GetBankAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/bank_account_get_by_id.json");

            var client = GetMockClient(content);
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);
            const string id = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            var response = repo.GetBankAccountById(id);
            client.VerifyAll();
            var first = response.Values.First();
            var firstJson = JsonConvert.SerializeObject(first);
            var gotAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(firstJson);

            Assert.AreEqual(id, gotAccount["id"]);
        }
        
        [Test]
        public void GetBankAccountEmptyId()
        {
            var client = GetMockClient("");
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.GetBankAccountById(string.Empty));
        }
        
        [Test]
        public void GetUserForBankAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/bank_account_get_users.json");

            var client = GetMockClient(content);
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);
            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var resp = repo.GetUserForBankAccount("ec9bf096-c505-4bef-87f6-18822b9dbf2c");
            client.VerifyAll();
            Assert.IsNotNull(resp);

            var users = resp["users"];
            Assert.IsNotNull(users);
            var user = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(users));
            Assert.AreEqual(userId, user["id"]);
        }
        
        [Test]
        public void DeleteBankAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/bank_account_delete.json");

            var client = GetMockClient(content);
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);

            var result = repo.DeleteBankAccount("e923013e-61e9-4264-9622-83384e13d2b9");
            client.VerifyAll();
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateRoutingNumberSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/bank_account_validate_routing_number.json");
            var client = GetMockClient(content);
            var repo = new BankAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<BankAccountRepository>().Object);
            var resp = repo.ValidateRoutingNumber("122235821");
            client.VerifyAll();

            var result = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["routing_number"]));
            Assert.AreEqual("US BANK NA", result["customer_name"]);
        }
    }
}
