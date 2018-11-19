using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicTransactionTest : AbstractTest
    {
        [Test]
        public void TransactionDeserialization()
        {
            var jsonStr = "{\"id\": \"8d8237c2-8598-4100-9fa5-f4ced75e7d76\",\"created_at\": \"2014-12-29T09:40:47.046Z\",\"updated_at\": \"2014-12-29T09:40:47.489Z\",\"description\": \"Buyer Fee @ 10%\",\"amount\": 5000,\"currency\":\"USD\",\"type\":\"debit\",\"from\": \"Escrow Vault\",\"to\": \"Awesome Websites\",\"related\": {\"transactions\":\"6a5525cf-e82f-40e7-995a-ad747185052a\"},\"links\":{\"self\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76\",\"users\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76/users\",\"fees\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76/fees\"}}";
            var transaction = JsonConvert.DeserializeObject<IDictionary<string,object>>(jsonStr);
            Assert.AreEqual("8d8237c2-8598-4100-9fa5-f4ced75e7d76", (string)transaction["id"]);
        }

        [Test]
        public void ListTransactionsSuccessful()
        {
            //First, create a user, so we'll have at least one 
            var content = File.ReadAllText("./Fixtures/transactions_list.json");
            var client = GetMockClient(content);
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);
            //Then, list users
            var transactions = repo.ListTransactions(200);
            Assert.IsNotNull(transactions);
        }

        [Test]
        public void ListTransactionsNegativeParams()
        {

            var client = GetMockClient("");
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.ListTransactions(-10, -20));
        }

        [Test]
        public void ListTransactionsTooHighLimit()
        {
            var client = GetMockClient("");
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.ListTransactions(201));
        }

        [Test]
        public void ShowTransactionWalletAccount() 
        {
            var content = File.ReadAllText("./Fixtures/transactions_show_wallet_account.json");
            var client = GetMockClient(content);
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);

            var response = repo.ShowTransactionWalletAccount("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var accounts = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["wallet_accounts"]));

            Assert.NotNull(accounts);
            Assert.AreEqual("4f4a9428-5fdd-4b4a-a7e3-0919cbba5e20", accounts["id"]);
        }

        [Test]
        public void ShowTransactionBankAccount()
        {
            var content = File.ReadAllText("./Fixtures/transactions_show_bank_account.json");
            var client = GetMockClient(content);
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);

            var response = repo.ShowTransactionBankAccount("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var accounts = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["transactions"]));

            Assert.NotNull(accounts);
            Assert.AreEqual("2f2870f5-7c8f-45cb-8aeb-95190d54f125", accounts["id"]);
        }

        [Test]
        public void ShowTransactionCardAccount()
        {
            var content = File.ReadAllText("./Fixtures/transactions_show_card_account.json");
            var client = GetMockClient(content);
            var repo = new TransactionRepository(client.Object, GetMockSettings().Object, GetMockLogger<TransactionRepository>().Object);

            var response = repo.ShowTransactionCardAccount("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var accounts = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["card_accounts"]));

            Assert.NotNull(accounts);
            Assert.AreEqual("930a7f78-6bf6-4f33-8cfc-b82c787b5f83", accounts["id"]);
        }

        [Test]
        [Ignore("No reason")]
        public void ShowTransactionPayPalAccount()
        {
            Assert.Fail("No fixture yet!");
        }
    }
}
