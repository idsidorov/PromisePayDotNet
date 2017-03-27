using Newtonsoft.Json;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.DTO;
using System;
using System.IO;
using Xunit;

namespace PromisePayDotNet.Tests
{
    public class CardAccountTest : AbstractTest
    {
        [Fact]
        public void CardAccountDeserialization()
        {
            const string jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-26T06:28:55.559Z\", \"updated_at\": \"2015-04-26T06:28:55.559Z\", \"id\": \"ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"currency\": \"USD\", \"card\": { \"type\": \"visa\", \"full_name\": \"Joe Frio\", \"number\": \"XXXX-XXXX-XXXX-1111\", \"expiry_month\": \"5\", \"expiry_year\": \"2016\" }, \"links\": { \"self\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19\", \"users\": \"/card_accounts/ea464d25-fc9a-4887-861a-3d8ec2e12c19/users\" } }";
            var cardAccount = JsonConvert.DeserializeObject<CardAccount>(jsonStr);
            Assert.Equal("ea464d25-fc9a-4887-861a-3d8ec2e12c19", cardAccount.Id);
            Assert.Equal("USD", cardAccount.Currency);
            Assert.Equal("Joe Frio", cardAccount.Card.FullName);
        }

        [Fact]
        public void CreateCardAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/card_account_create.json");

            var client = GetMockClient(content);
            var repo = Get<ICardAccountRepository>(client.Object);

            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new CardAccount
            {
                UserId = userId,
                Active = true,
                Card = new Card
                {
                    FullName = "Batman",
                    ExpiryMonth = "11",
                    ExpiryYear = "2020",
                    Number = "4111111111111111",
                    Type = "visa",
                    CVV = "123"
                }
            };
            var createdAccount = repo.CreateCardAccount(account);
            client.VerifyAll();
            Assert.NotNull(createdAccount);
            Assert.NotNull(createdAccount.Id);
            Assert.Equal("AUD", createdAccount.Currency); // It seems that currency is determined by country
            Assert.NotNull(createdAccount.CreatedAt);
            Assert.NotNull(createdAccount.UpdatedAt);
           
        }

        [Fact]
        public void GetCardAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/card_account_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<ICardAccountRepository>(client.Object);
            var gotAccount = repo.GetCardAccountById("25d34744-8ef0-46a4-8b18-2a8322933cd1");
            client.VerifyAll();
            Assert.Equal("25d34744-8ef0-46a4-8b18-2a8322933cd1", gotAccount.Id);
        }

        [Fact]
        public void GetCardAccountEmptyId()
        {
            var client = GetMockClient("");
            var repo = Get<ICardAccountRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.GetCardAccountById(string.Empty));
        }

        [Fact]
        public void GetUserForCardAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/card_account_get_users.json");

            var client = GetMockClient(content);
            var repo = Get<ICardAccountRepository>(client.Object);
            var gotUser = repo.GetUserForCardAccount("25d34744-8ef0-46a4-8b18-2a8322933cd1");

            client.VerifyAll();

            Assert.NotNull(gotUser);
            Assert.Equal("1", gotUser.Id);
        }

        [Fact]
        public void DeleteCardAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/card_account_delete.json");

            var client = GetMockClient(content);
            var repo = Get<ICardAccountRepository>(client.Object);
            const string id = "25d34744-8ef0-46a4-8b18-2a8322933cd1";

            var result = repo.DeleteCardAccount(id);
            client.VerifyAll();
            Assert.True(result);
        }

    }
}
