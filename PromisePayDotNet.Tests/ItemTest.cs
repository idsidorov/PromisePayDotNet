using System.IO;
using System.Net;
using Moq;
using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Enums;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Implementations;
using System;
using System.Linq;
using PromisePayDotNet.Internals;
using PromisePayDotNet.Abstractions;

namespace PromisePayDotNet.Tests
{
    public class ItemTest : AbstractTest
    {
        [Fact]
        public void ItemDeserialization()
        {
            var jsonStr = "        {            \"id\": \"293\",            \"name\": \"Testing\",            \"description\": \"This is the description\",            \"created_at\": \"2015-05-01T04:42:25.595Z\",            \"updated_at\": \"2015-05-01T04:42:25.595Z\",            \"state\": \"pending\",            \"deposit_reference\": \"9140122157\",            \"payment_type_id\": 1,            \"status\": 22000,\"amount\": 6000,            \"buyer_name\": \"Joe Frio\",            \"buyer_country\": \"USA\",            \"buyer_email\": \"joe.test@promisepay.com\",\"seller_name\": \"Julie Boatsman\",            \"seller_country\": \"USA\",            \"seller_email\": \"julie.test@promisepay.com\",            \"currency\": \"USD\",\"links\": {                \"self\": \"/items/293\",                \"buyers\": \"/items/293/buyers\",                \"sellers\": \"/items/293/sellers\",                \"status\": \"/items/293/status\",                \"fees\": \"/items/293/fees\",                \"transactions\": \"/items/293/transactions\"            }        }";
            var item = JsonConvert.DeserializeObject<Item>(jsonStr);
            Assert.Equal("293", item.Id);
        }

        [Fact]
        public void CreateItemSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_create.json");

            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            const string id = "5e81906c-e14b-42a8-952f-4a0d1f1a4bb8";
            const string buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            const string sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
            var item = new Item
            {
                Id = id,
                Name = "Test Item #1",
                Amount = 1000,
                PaymentType = PaymentType.Express,
                BuyerId = buyerId, //optional field
                SellerId = sellerId, //optional field
                //No fee at this stage, optional field
                Description = "Test item #1 description"
            };
            var createdItem = repo.CreateItem(item);
            Assert.Equal(item.Id, createdItem.Id);
            Assert.Equal(item.Name, createdItem.Name);
            Assert.Equal(item.Amount, createdItem.Amount);
            Assert.Equal(item.PaymentType, createdItem.PaymentType);
            Assert.Equal(item.Description, createdItem.Description);
        }

        [Fact]
        public void ListAllItemsSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_list.json");

            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            //Then, list items
            var items = repo.ListItems(200);

            Assert.NotNull(items);
            Assert.True(items.Any());
        }

        [Fact]
        public void ListAllItemsNegativeParams()
        {
            var client = GetMockClient("");
            var repo = Get<IItemRepository>(client.Object);
            //Then, list items
            Assert.Throws<ArgumentException>(() => repo.ListItems(-10, -10));
        }

        [Fact]
        public void ListAllItemsTooHighLimit()
        {
            var client = GetMockClient("");
            var repo = Get<IItemRepository>(client.Object);

            //Then, list items
            Assert.Throws<ArgumentException>(() => repo.ListItems(500));
        }

        [Fact]
        public void GetItemSuccessful()
        {
            //First, create a user with known id
            var content = Files.ReadAllText("./Fixtures/items_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);

            const string id = "5e81906c-e14b-42a8-952f-4a0d1f1a4bb8";
            var gotItem = repo.GetItemById(id);

            Assert.NotNull(gotItem);
            Assert.Equal(id, gotItem.Id);
        }

        [Fact]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void GetItemMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/items_not_found.json");
            var response = new Mock<RestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>())).ReturnsAsync(response.Object);
            var repo = Get<IItemRepository>(client.Object); 
            var id = Guid.NewGuid().ToString();
            Assert.Throws<UnauthorizedException>(() => repo.GetItemById(id));
        }

        [Fact]
        public void DeleteItemSuccessful()
        {
            var id = "db3d95aa-2e35-4d87-95b4-5c9b41ba7346";
            var content = Files.ReadAllText("./Fixtures/items_delete.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            Assert.True(repo.DeleteItem(id));
            client.VerifyAll();
        }

        [Fact]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void DeleteItemMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/items_delete_unsuccessful.json");
            var response = new Mock<RestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>())).ReturnsAsync(response.Object);
            var repo = Get<IItemRepository>(client.Object);
            var id = Guid.NewGuid().ToString();
            Assert.Throws<UnauthorizedException>(() => repo.DeleteItem(id));
        }

        [Fact]
        public void EditItemSuccessful()
        {
            //First, create a item we'll work with
            var content = Files.ReadAllText("./Fixtures/items_edit.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);

            var id = "172500df-0f2a-4e43-8fe7-f4a36dfbd1a2";
            var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
            var item = new Item
            {
                Id = id,
                Name = "Test Item #1",
                Amount = 1000,
                PaymentType = PaymentType.Express,
                BuyerId = buyerId, //optional field
                SellerId = sellerId, //optional field
                //No fee at this stage, optional field
                Description = "Test item #1 description"
            };

            //Now, try to edit newly created item
            item.Name = "Test123";
            item.Description = "Test123";
            var updatedItem = repo.UpdateItem(item);

            Assert.Equal("Test123", updatedItem.Name);
            Assert.Equal("Test123", updatedItem.Description);
        }

        [Fact]
        public void EditItemMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/items_edit_unsuccessful.json");
            var response = new Mock<RestResponse>(MockBehavior.Strict);
            response.SetupGet(x => x.Content).Returns(content);
            response.SetupGet(x => x.ResponseUri).Returns(new Uri("http://google.com"));
            response.SetupGet(x => x.StatusDescription).Returns("Unauthorized");
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var client = new Mock<IRestClient>(MockBehavior.Strict);
            client.SetupSet(x => x.BaseUrl = It.IsAny<Uri>());
            client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>());
            client.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>())).ReturnsAsync(response.Object);
            var repo = Get<IItemRepository>(client.Object);
            var id = Guid.NewGuid().ToString();
            var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
            var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
            var item = new Item
            {
                Id = id,
                Name = "Test Item #1",
                Amount = 1000,
                PaymentType = PaymentType.Express,
                BuyerId = buyerId, //optional field
                SellerId = sellerId, //optional field
                //No fee at this stage, optional field
                Description = "Test item #1 description"
            };

            Assert.Throws<UnauthorizedException>(() => repo.UpdateItem(item));
        }

        [Fact]
        public void ListTransactionsForItem()
        {
            var content = Files.ReadAllText("./Fixtures/items_list_transactions.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);

            var transactions = repo.ListTransactionsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(transactions);
        }

        [Fact]
        public void GetStatusForItem()
        {
            var content = Files.ReadAllText("./Fixtures/items_get_status.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object); 
            var status = repo.GetStatusForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(status);
        }

        [Fact]
        public void ListFeesForItem()
        {
            var content = Files.ReadAllText("./Fixtures/items_list_fees.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object); 
            var fees = repo.ListFeesForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(fees);
        }

        [Fact]
        public void GetBuyerForItemSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_get_buyer.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            var buyer = repo.GetBuyerForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(buyer);
        }

        [Fact]
        public void GetSellerForItemSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_get_seller.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            var sellers = repo.GetSellerForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(sellers);
        }

        [Fact]
        public void GetWireDetailsForItemSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_get_wire_details.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);

            var wireDetails = repo.GetWireDetailsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(wireDetails);
        }

        [Fact]
        public void GetBPayDetailsForItemSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/items_get_bpay_details.json");
            var client = GetMockClient(content);
            var repo = Get<IItemRepository>(client.Object);
            var bPayDetails = repo.GetBPayDetailsForItem("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
            Assert.NotNull(bPayDetails);
        }
    }
}
