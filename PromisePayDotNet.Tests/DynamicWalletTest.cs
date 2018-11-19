using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PromisePayDotNet.Tests
{
    public class DynamicWalletTest : AbstractTest
    {
        [Test]
        public void ShowSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/wallets_show.json");
            var client = GetMockClient(content);

            var repo = new WalletRepository(client.Object, GetMockSettings().Object, GetMockLogger<WalletRepository>().Object);
            var response = repo.ShowWalletAccount("385b50bb-237a-42cb-9382-22953e191ae6");
            client.VerifyAll();

            Assert.IsNotNull(response);
            var walletAccounts = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["wallet_accounts"]));
            Assert.AreEqual("385b50bb-237a-42cb-9382-22953e191ae6", walletAccounts["id"]);
        }

        [Test]
        public void WithdrawSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/wallets_withdraw_funds.json");
            var client = GetMockClient(content);

            var repo = new WalletRepository(client.Object, GetMockSettings().Object, GetMockLogger<WalletRepository>().Object);
            var response = repo.WithdrawFunds("385b50bb-237a-42cb-9382-22953e191ae6");
            client.VerifyAll();

            Assert.IsNotNull(response);
            var disbursements = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["disbursements"]));
            Assert.AreEqual("ad688d54-6791-4f1d-add7-88fbd89b70d1", disbursements["id"]);
        }

        [Test]
        public void DepositSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/wallets_deposit_funds.json");
            var client = GetMockClient(content);

            var repo = new WalletRepository(client.Object, GetMockSettings().Object, GetMockLogger<WalletRepository>().Object);
            var response = repo.WithdrawFunds("385b50bb-237a-42cb-9382-22953e191ae6");
            client.VerifyAll();

            Assert.IsNotNull(response);
            var disbursements = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["disbursements"]));
            Assert.AreEqual("210bc8ba-d646-4e35-b029-a1959e4ca8c5", disbursements["id"]);
        }

        [Test]
        public void ShowWalletAccountUserSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/wallets_show_user_account.json");
            var client = GetMockClient(content);

            var repo = new WalletRepository(client.Object, GetMockSettings().Object, GetMockLogger<WalletRepository>().Object);
            var response = repo.ShowWalletAccountUser("385b50bb-237a-42cb-9382-22953e191ae6");
            client.VerifyAll();

            Assert.IsNotNull(response);
            var users = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["users"]));
            Assert.AreEqual("samuel.seller@promisepay.com", users["email"]);
        }

        
    }
}
