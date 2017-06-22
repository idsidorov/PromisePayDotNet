using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace PromisePayDotNet.Implementations
{
    internal class ItemRepository : AbstractRepository, IItemRepository
    {
        public ItemRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<ItemRepository>(), options)
        {
        }


        public async Task<IEnumerable<Item>> ListItemsAsync(int limit = 10, int offset = 0)
        {
            AssertListParamsCorrect(limit, offset);
            var request = new RestRequest("/items", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var userCollection = dict["items"];
                return JsonConvert.DeserializeObject<List<Item>>(JsonConvert.SerializeObject(userCollection));
            }
            return new List<Item>();
        }

        public async Task<Item> GetItemByIdAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Item>>(response.Content).Values.First();
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            var request = new RestRequest("/items", Method.POST);
            request.AddParameter("id", item.Id);
            request.AddParameter("name", item.Name);
            request.AddParameter("amount", item.Amount);
            request.AddParameter("payment_type", (int)item.PaymentType);
            request.AddParameter("buyer_id", item.BuyerId);
            request.AddParameter("seller_id", item.SellerId);
            request.AddParameter("fee_ids", item.FeeIds);
            request.AddParameter("description", item.Description);
             var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Item>>(response.Content).Values.First();
        }

        public async Task<bool> DeleteItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}", Method.DELETE);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            var request = new RestRequest("/items/{id}", Method.PATCH);
            request.AddUrlSegment("id", item.Id);

            request.AddParameter("amount", item.Amount);
            request.AddParameter("name", item.Name);
            request.AddParameter("description", item.Description);
            request.AddParameter("buyer_id", item.BuyerId);
            request.AddParameter("seller_id", item.SellerId);
            request.AddParameter("fee_ids", item.FeeIds);

            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Item>>(response.Content).Values.First();
        }

        public async Task<IEnumerable<Transaction>> ListTransactionsForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/transactions", Method.GET);
            request.AddUrlSegment("id", itemId);
            RestResponse response;
            try
            {
                response = await SendRequestAsync(Client, request);
            }
            catch (ApiErrorsException e)
            {
                if (e.Errors.Count == 1 && e.Errors.Values.First().First() == "no transaction found")
                {
                    return new List<Transaction>();
                }
                throw;
            }
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("transactions"))
            {
                var itemCollection = dict["transactions"];
                return JsonConvert.DeserializeObject<List<Transaction>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<Transaction>();
        }

        public async Task<ItemStatus> GetStatusForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/status", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                return JsonConvert.DeserializeObject<ItemStatus>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public async Task<IEnumerable<Fee>> ListFeesForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/fees", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("fees"))
            {
                var itemCollection = dict["fees"];
                return JsonConvert.DeserializeObject<List<Fee>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<Fee>();
        }

        public async Task<User> GetBuyerForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/buyers", Method.GET);
            request.AddUrlSegment("id", itemId);
            RestResponse response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var itemCollection = dict["users"];
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public async Task<User> GetSellerForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/sellers", Method.GET);
            request.AddUrlSegment("id", itemId);
            RestResponse response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var itemCollection = dict["users"];
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(itemCollection));
            }
            return null;
        }

        public async Task<WireDetails> GetWireDetailsForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/wire_details", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var details =  JsonConvert.DeserializeObject<DetailsContainer>(JsonConvert.SerializeObject(itemCollection));
                return details.WireDetails;
            }
            return null;
        }

        public async Task<BPayDetails> GetBPayDetailsForItemAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/bpay_details", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var details = JsonConvert.DeserializeObject<DetailsContainer>(JsonConvert.SerializeObject(itemCollection));
                return details.BPayDetails;
            }
            return null;
        }

        public async Task<Item> MakePaymentAsync(string itemId, string accountId)
        {
            AssertIdNotNull(itemId);
            AssertIdNotNull(accountId);
            var request = new RestRequest("/items/:id/make_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("account_id", accountId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> RequestPaymentAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/request_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> ReleasePaymentAsync(string itemId, int releaseAmount)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/release_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("release_amount", releaseAmount);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> RequestReleaseAsync(string itemId, int releaseAmount)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/request_release", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("release_amount", releaseAmount);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> CancelAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/cancel", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> AcknowledgeWireAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/acknowledge_wire", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> AcknowledgePayPalAsync(string itemId)
        {
            //NOTE: Not documented!
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/acknowledge_paypal", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> RevertWireAsync(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/revert_wire", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> RequestRefundAsync(string itemId, string refundAmount, string refundMessage)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/request_refund", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("refund_amount", refundAmount);
            request.AddParameter("refund_message", refundMessage);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }

        public async Task<Item> RefundAsync(string itemId, string refundAmount, string refundMessage)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/:id/refund", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("refund_amount", refundAmount);
            request.AddParameter("refund_message", refundMessage);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                var item = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(itemCollection));
                return item;
            }
            return null;
        }
    }
}
