using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using PromisePayDotNet.Exceptions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PromisePayDotNet.Tests
{
    public class DynamicItemTest : AbstractTest
    {
        [Test]
        public void ItemDeserialization()
        {
            var jsonStr = "        {            \"id\": \"293\",            \"name\": \"Testing\",            \"description\": \"This is the description\",            \"created_at\": \"2015-05-01T04:42:25.595Z\",            \"updated_at\": \"2015-05-01T04:42:25.595Z\",            \"state\": \"pending\",            \"deposit_reference\": \"9140122157\",            \"payment_type_id\": 1,            \"status\": 22000,\"amount\": 6000,            \"buyer_name\": \"Joe Frio\",            \"buyer_country\": \"USA\",            \"buyer_email\": \"joe.test@promisepay.com\",\"seller_name\": \"Julie Boatsman\",            \"seller_country\": \"USA\",            \"seller_email\": \"julie.test@promisepay.com\",            \"currency\": \"USD\",\"links\": {                \"self\": \"/items/293\",                \"buyers\": \"/items/293/buyers\",                \"sellers\": \"/items/293/sellers\",                \"status\": \"/items/293/status\",                \"fees\": \"/items/293/fees\",                \"transactions\": \"/items/293/transactions\"            }        }";
            var item = JsonConvert.DeserializeObject<IDictionary<string,object>>(jsonStr);
            Assert.AreEqual("293", item["id"]);
        }
        
        [Test]
        public void CreateItemSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_create.json");

            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            const string id = "5e81906c-e14b-42a8-952f-4a0d1f1a4bb8";
            const string buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            const string sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
            var item = new Dictionary<string, object>
            {
                {"id" , id},
                {"name" , "Test Item #1"},
                {"amount" , 1000},
                {"payment_type" , 2},
                {"buyer_id" , buyerId},
                {"seller_id" , sellerId},
                {"fee_ids" , ""},
                {"description" , "Test item #1 description"}
            };
            var resp = repo.CreateItem(item);
            var createdItem = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));

            Assert.AreEqual(item["id"], createdItem["id"]);
            Assert.AreEqual(item["name"], createdItem["name"]);
            Assert.AreEqual(item["amount"], createdItem["amount"]);
            Assert.AreEqual(item["payment_type"], createdItem["payment_type_id"]);
            Assert.AreEqual(item["description"], createdItem["description"]);
        }
        
        [Test]
        public void ListAllItemsSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_list.json");

            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            //Then, list items
            var items = repo.ListItems(200);

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
        }

        [Test]
        public void ListAllItemsNegativeParams()
        {
            var client = GetMockClient("");
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            //Then, list items
            Assert.Throws<ArgumentException>(() => repo.ListItems(-10, -10));
        }

        [Test]
        public void ListAllItemsTooHighLimit()
        {
            var client = GetMockClient("");
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            //Then, list items
            Assert.Throws<ArgumentException>(() => repo.ListItems(500));
        }
        
        [Test]
        public void GetItemSuccessful()
        {
            //First, create a user with known id
            var content = File.ReadAllText("./Fixtures/items_get_by_id.json");

            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            const string id = "5e81906c-e14b-42a8-952f-4a0d1f1a4bb8";
            var resp = repo.GetItemById(id);
            var gotItem = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.IsNotNull(gotItem);
            Assert.AreEqual(id, gotItem["id"]);
        }
        
        [Test]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void GetItemMissingId()
        {
            var content = File.ReadAllText("./Fixtures/items_not_found.json");
            var response = new Mock<IRestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var id = Guid.NewGuid().ToString();
            Assert.Throws<UnauthorizedException>(() => repo.GetItemById(id));
        }
        
        [Test]
        public void DeleteItemSuccessful()
        {
            var id = "db3d95aa-2e35-4d87-95b4-5c9b41ba7346";
            var content = File.ReadAllText("./Fixtures/items_delete.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            Assert.IsTrue(repo.DeleteItem(id));
            client.VerifyAll();
        }
        
        [Test]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void DeleteItemMissingId()
        {
            var content = File.ReadAllText("./Fixtures/items_delete_unsuccessful.json");
            var response = new Mock<IRestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var id = Guid.NewGuid().ToString();
            Assert.Throws<UnauthorizedException>(() => repo.DeleteItem(id));
        }
        
        [Test]
        public void EditItemSuccessful()
        {
            //First, create a item we'll work with
            var content = File.ReadAllText("./Fixtures/items_edit.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var id = "172500df-0f2a-4e43-8fe7-f4a36dfbd1a2";
            var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before

            var item = new Dictionary<string, object>
            {
                {"id" , id},
                {"name" , "Test Item #1"},
                {"amount" , 1000},
                {"payment_type" , 2},
                {"buyer_id" , buyerId},
                {"seller_id" , sellerId},
                {"fee_ids" , ""},
                {"description" , "Test item #1 description"}
            };
            
            //Now, try to edit newly created item
            item["name"] = "Test123";
            item["description"] = "Test123";
            var resp = repo.UpdateItem(item);
            var updatedItem = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual("Test123", updatedItem["name"]);
            Assert.AreEqual("Test123", updatedItem["description"]);
        }
        
        [Test]
        public void EditItemMissingId()
        {
            var content = File.ReadAllText("./Fixtures/items_edit_unsuccessful.json");
            var response = new Mock<IRestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var id = Guid.NewGuid().ToString();
            var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
            var item = new Dictionary<string, object>
            {
                {"id" , id},
                {"name" , "Test Item #1"},
                {"amount" , 1000},
                {"payment_type" , 2},
                {"buyer_id" , buyerId},
                {"seller_id" , sellerId},
                {"fee_ids" , ""},
                {"description" , "Test item #1 description"}
            };

            Assert.Throws<UnauthorizedException>(() => repo.UpdateItem(item));
        }

        [Test]
        public void ListTransactionsForItem()
        {
            var content = File.ReadAllText("./Fixtures/items_list_transactions.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var transactions = repo.ListTransactionsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(transactions);
        }

        [Test]
        public void GetStatusForItem()
        {
            var content = File.ReadAllText("./Fixtures/items_get_status.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var status = repo.GetStatusForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(status);
        }

        [Test]
        public void ListFeesForItem()
        {
            var content = File.ReadAllText("./Fixtures/items_list_fees.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var fees = repo.ListFeesForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(fees);
        }

        [Test]
        public void GetBuyerForItemSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_get_buyer.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var buyer = repo.GetBuyerForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(buyer);
        }

        [Test]
        public void GetSellerForItemSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_get_seller.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var sellers = repo.GetSellerForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(sellers);
        }

        [Test]
        public void GetWireDetailsForItemSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_get_wire_details.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var wireDetails = repo.GetWireDetailsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(wireDetails);
        }

        [Test]
        public void GetBPayDetailsForItemSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_get_bpay_details.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var bPayDetails = repo.GetBPayDetailsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(bPayDetails);
        }

        [Test]
        public void ListBatchTransactionsSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/items_list_batch_transactions.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);
            var response = repo.ListBatchTransactions("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.IsNotNull(response["batch_transactions"]);
        }        
    }
}
