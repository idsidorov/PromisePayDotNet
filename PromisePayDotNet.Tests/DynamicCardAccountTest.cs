using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class DynamicCardAccountTest : AbstractTest
    {
        [Fact]
        public void CardAccountDeserialization()
        {
            const string jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-26T06:28:55.559Z\", \"updated_at\": \"2015-04-26T06:28:55.559Z\", \"id\": \"ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"currency\": \"USD\", \"card\": { \"type\": \"visa\", \"full_name\": \"Joe Frio\", \"number\": \"XXXX-XXXX-XXXX-1111\", \"expiry_month\": \"5\", \"expiry_year\": \"2016\" }, \"links\": { \"self\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"users\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19/users\" } }";
            var cardAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.Equal("ea464d25-fc9a-4887-861a-3d8ec2e12c19", (string)cardAccount["id"]);
            Assert.Equal("USD", (string)cardAccount["currency"]);
            var card = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(cardAccount["card"]));
            
            Assert.Equal("Joe Frio", card["full_name"]);
        }

        [Fact]
        public void CreateCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_create.json");

            var client = GetMockClient(content);
            var repo = Get<CardAccountRepository>(client.Object);

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
            var createdAccount = repo.CreateCardAccount(account);
            client.VerifyAll();
            Assert.NotNull(createdAccount);
            Assert.NotNull(createdAccount["id"]);
            Assert.Equal("AUD", (string)createdAccount["currency"]); // It seems that currency is determined by country
            Assert.NotNull(createdAccount["created_at"]);
            Assert.NotNull(createdAccount["updated_at"]);
        }

        [Fact]
        public void GetCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<CardAccountRepository>(client.Object);
            var gotAccount = repo.GetCardAccountById("25d34744-8ef0-46a4-8b18-2a8322933cd1");
            client.VerifyAll();
            Assert.Equal("25d34744-8ef0-46a4-8b18-2a8322933cd1", gotAccount["id"]);
        }
        
        [Fact]
        public void GetCardAccountEmptyId()
        {
            var client = GetMockClient("");
            var repo = Get<CardAccountRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.GetCardAccountById(string.Empty));
        }
        
        [Fact]
        public void GetUserForCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_get_users.json");

            var client = GetMockClient(content);
            var repo = Get<CardAccountRepository>(client.Object);
            var gotUser = repo.GetUserForCardAccount("25d34744-8ef0-46a4-8b18-2a8322933cd1");

            client.VerifyAll();

            Assert.NotNull(gotUser);
            Assert.Equal("1", gotUser["id"]);
        }
        
        [Fact]
        public void DeleteCardAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/card_account_delete.json");

            var client = GetMockClient(content);
            var repo = Get<CardAccountRepository>(client.Object);
            const string id = "25d34744-8ef0-46a4-8b18-2a8322933cd1";

            var result = repo.DeleteCardAccount(id);
            client.VerifyAll();
            Assert.True(result);
        }
        
    }
}
