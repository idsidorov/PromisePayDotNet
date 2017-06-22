using System.IO;
using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Implementations;
using System;
using PromisePayDotNet.Internals;
using PromisePayDotNet.Abstractions;

namespace PromisePayDotNet.Tests
{
    public class TransactionTest : AbstractTest
    {
        [Fact]
        public void TransactionDeserialization()
        {
            var jsonStr = "{\"id\": \"8d8237c2-8598-4100-9fa5-f4ced75e7d76\",\"created_at\": \"2014-12-29T09:40:47.046Z\",\"updated_at\": \"2014-12-29T09:40:47.489Z\",\"description\": \"Buyer Fee @ 10%\",\"amount\": 5000,\"currency\":\"USD\",\"type\":\"debit\",\"from\": \"Escrow Vault\",\"to\": \"Awesome Websites\",\"related\": {\"transactions\":\"6a5525cf-e82f-40e7-995a-ad747185052a\"},\"links\":{\"self\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76\",\"users\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76/users\",\"fees\":\"/transactions/8d8237c2-8598-4100-9fa5-f4ced75e7d76/fees\"}}";
            var transaction = JsonConvert.DeserializeObject<Transaction>(jsonStr);
            Assert.Equal("8d8237c2-8598-4100-9fa5-f4ced75e7d76", transaction.Id);
        }

        [Fact]
        public void ListTransactionsSuccessful()
        {
            //First, create a user, so we'll have at least one 
            var content = Files.ReadAllText("./Fixtures/transactions_list.json");
            var client = GetMockClient(content);
            var repo = Get<ITransactionRepository>(client.Object);
            //Then, list users
            var transactions = repo.ListTransactions(200);
            Assert.NotNull(transactions);
        }

        [Fact]
        public void ListTransactionsNegativeParams()
        {
            
            var client = GetMockClient("");
            var repo = Get<ITransactionRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.ListTransactions(-10, -20));
        }

        [Fact]
        public void ListTransactionsTooHighLimit()
        {
            var client = GetMockClient("");
            var repo = Get<ITransactionRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.ListTransactions(201));
        }
    }
}
