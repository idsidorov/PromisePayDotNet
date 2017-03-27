using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class DynamicPayPalAccountTest : AbstractTest
    {
        [Fact]
        public void PayPalAccountDeserialization()
        {
            var jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-25T12:31:39.324Z\", \"updated_at\": \"2015-04-25T12:31:39.324Z\", \"id\": \"70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"currency\": \"USD\", \"paypal\": { \"email\": \"test.me@promisepay.com\" }, \"links\": { \"self\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"users\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779/users\" } }";
            var payPalAccount = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.Equal("70d93fe3-6c2e-4a1c-918f-13b8e7bb3779", (string)payPalAccount["id"]);
            Assert.Equal("USD", (string)payPalAccount["currency"]);
            var paypal = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(payPalAccount["paypal"]));
            Assert.Equal("test.me@promisepay.com", (string)paypal["email"]);
        }

        [Fact]
        public void CreatePayPalAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = Get<PayPalAccountRepository>(client.Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new Dictionary<string, object>
            { { "user_id", userId },
                {"active" , true},
                {"paypal" , new Dictionary<string, object>
                {
                    {"email", "aaa@bbb.com"}
                }}
            };
            var createdAccount = repo.CreatePayPalAccount(account);
            Assert.NotNull(createdAccount);
            Assert.NotNull(createdAccount["id"]);
            Assert.Equal("AUD", (string)createdAccount["currency"]); // It seems that currency is determined by country
            Assert.NotNull(createdAccount["created_at"]);
            Assert.NotNull(createdAccount["updated_at"]);

        }

        [Fact]
        public void GetPayPalAccountSuccessfully()
        {
            var id = "cd2ab053-25e5-491a-a5ec-0c32dbe76efa";
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = Get<PayPalAccountRepository>(client.Object);

            var gotAccount = repo.GetPayPalAccountById(id);

            Assert.Equal(id, (string)gotAccount["id"]);
        }

        [Fact]
        public void GetPayPalAccountEmptyId()
        {
            var client = GetMockClient("");

            var repo = Get<PayPalAccountRepository>(client.Object);

            Assert.Throws<ArgumentException>(() => repo.GetPayPalAccountById(string.Empty));
        }

        [Fact]
        public void GetUserForPayPalAccountSuccessfully()
        {
            var id = "3a780d4a-5de0-409c-9587-080930ddea3c";

            var content = File.ReadAllText("./Fixtures/paypal_account_get_users.json");
            var client = GetMockClient(content);
            var repo = Get<PayPalAccountRepository>(client.Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before

            var gotUser = repo.GetUserForPayPalAccount(id);

            Assert.NotNull(gotUser);

            Assert.Equal(userId, gotUser["id"]);
        }

        [Fact]
        public void DeletePayPalAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/paypal_account_delete.json");
            var client = GetMockClient(content);
            var repo = Get<PayPalAccountRepository>(client.Object);

            var result = repo.DeletePayPalAccount("cd2ab053-25e5-491a-a5ec-0c32dbe76efa");
            Assert.True(result);
        }

    }
}
