using Newtonsoft.Json;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.DTO;

namespace PromisePayDotNet.Tests
{
    public class DynamicBankAccountTest : AbstractTest
    {
        [Fact]
        public void BankAccountDeserialization()
        {
            const string jsonStr = "{ \"active\": true, \"created_at\": \"2015-04-26T06:24:19.248Z\", \"updated_at\": \"2015-04-26T06:24:19.248Z\", \"id\": \"8d65c86c-14f4-4abf-a979-eba0a87b283a\", \"currency\": \"USD\", \"bank\": { \"bank_name\": \"Test Me\", \"country\": \"AUS\", \"account_name\": \"Test Account\", \"routing_number\": \"XXXXXXX3\", \"account_number\": \"XXXX344\", \"holder_type\": \"personal\", \"account_type\": \"savings\" }, \"links\": { \"self\": \"/bank_accounts/8d65c86c-14f4-4abf-a979-eba0a87b283a\", \"users\": \"/bank_accounts/8d65c86c-14f4-4abf-a979-eba0a87b283a/users\" } }";
            var bankAccount = JsonConvert.DeserializeObject<BankAccount>(jsonStr);
            Assert.Equal("8d65c86c-14f4-4abf-a979-eba0a87b283a", (string)bankAccount.Id);
            Assert.Equal("USD", (string)bankAccount.Currency);
        }
        
        [Fact]
        public void CreateBankAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/bank_account_create.json");

            var client = GetMockClient(content);
            var repo = Get<IBankAccountRepository>(client.Object);

            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var account = new BankAccount
            {   UserId= userId ,
                Active = true ,
                Bank = new Bank
                {   BankName = "Test bank, inc",
                   AccountName = "Test account",
                   AccountNumber = "8123456789",
                   AccountType = "savings",
                    Country = "AUS",
                   HolderType = "personal",
                   RoutingNumber = "123456"
                }
            };
            var createdAccount = repo.CreateBankAccount(account);
            client.VerifyAll();
            Assert.NotNull(createdAccount);
            Assert.NotNull(createdAccount.Id);
            Assert.Equal("AUD", (string)createdAccount.Currency); // It seems that currency is determined by country
            Assert.NotNull(createdAccount.CreatedAt);
            Assert.NotNull(createdAccount.UpdatedAt);
            Assert.Equal("XXX789", (string)createdAccount.Bank.AccountNumber); //Account number is masked
        }
        
        [Fact]
        public void GetBankAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/bank_account_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<IBankAccountRepository>(client.Object);
            const string id = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            var gotAccount = repo.GetBankAccountById(id);
            client.VerifyAll();
            Assert.Equal(id, gotAccount.Id);
        }
        
        [Fact]
        public void GetBankAccountEmptyId()
        {
            var client = GetMockClient("");
            var repo = Get<IBankAccountRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.GetBankAccountById(string.Empty));
        }
        
        [Fact]
        public void GetUserForBankAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/bank_account_get_users.json");

            var client = GetMockClient(content);
            var repo = Get<IBankAccountRepository>(client.Object);
            const string userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var gotUser = repo.GetUserForBankAccount("ec9bf096-c505-4bef-87f6-18822b9dbf2c");
            client.VerifyAll();
            Assert.NotNull(gotUser);

            Assert.Equal(userId, gotUser.Id);
        }
        
        [Fact]
        public void DeleteBankAccountSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/bank_account_delete.json");

            var client = GetMockClient(content);
            var repo = Get<IBankAccountRepository>(client.Object);

            var result = repo.DeleteBankAccount("e923013e-61e9-4264-9622-83384e13d2b9");
            client.VerifyAll();
            Assert.True(result);
        }
        
    }
}
