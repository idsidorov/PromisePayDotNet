using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Implementations;
using RestSharp;
using System;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class PayPalAccountTest : AbstractTest
    {
        [Test]
        public void PayPalAccountDeserialization()
        {
            var jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-25T12:31:39.324Z\", \"updated_at\": \"2015-04-25T12:31:39.324Z\", \"id\": \"70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"currency\": \"USD\", \"paypal\": { \"email\": \"test.me@promisepay.com\" }, \"links\": { \"self\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779\", \"users\": \"/paypal_accounts/70d93fe3-6c2e-4a1c-918f-13b8e7bb3779/users\" } }";
            var payPalAccount = JsonConvert.DeserializeObject<PayPalAccount>(jsonStr);
            Assert.AreEqual("70d93fe3-6c2e-4a1c-918f-13b8e7bb3779", payPalAccount.Id);
            Assert.AreEqual("USD", payPalAccount.Currency);
            Assert.AreEqual("test.me@promisepay.com", payPalAccount.PayPal.Email);
        }

        [Test]
        public void CreatePayPalAccountSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new PayPalAccount
            {
                UserId = userId,
                Active = true,
                PayPal = new PayPal
                {
                    Email = "aaa@bbb.com"
                }
            };
            var createdAccount = repo.CreatePayPalAccount(account);
            Assert.IsNotNull(createdAccount);
            Assert.IsNotNull(createdAccount.Id);
            Assert.AreEqual("AUD", createdAccount.Currency); // It seems that currency is determined by country
            Assert.IsNotNull(createdAccount.CreatedAt);
            Assert.IsNotNull(createdAccount.UpdatedAt);

        }

        [Test]
        public void GetPayPalAccountSuccessfully()
        {
            var id = "cd2ab053-25e5-491a-a5ec-0c32dbe76efa";
            var content = File.ReadAllText("./Fixtures/paypal_account_create.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var gotAccount = repo.GetPayPalAccountById(id);

            Assert.AreEqual(id, gotAccount.Id);
        }

        [Test]
        public void GetPayPalAccountEmptyId()
        {
            var client = GetMockClient("");

            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            Assert.Throws<ArgumentException>(()=>repo.GetPayPalAccountById(string.Empty));
        }

        [Test]
        public void GetUserForPayPalAccountSuccessfully()
        {
            var id = "3a780d4a-5de0-409c-9587-080930ddea3c";

            var content = File.ReadAllText("./Fixtures/paypal_account_get_users.json");
            var client = GetMockClient(content);
            var repo = new PayPalAccountRepository(client.Object, GetMockSettings().Object, GetMockLogger<PayPalAccountRepository>().Object);

            var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before

            var gotUser = repo.GetUserForPayPalAccount(id);

            Assert.IsNotNull(gotUser);

            Assert.AreEqual(userId, gotUser.Id);
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
