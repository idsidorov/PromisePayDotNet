using Newtonsoft.Json;
using PromisePayDotNet.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class ItemRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                    PromisePayDotNet.Dynamic.Interfaces.IItemRepository
    {
        public ItemRepository(IRestClient client, ISettings settings, ILogger<ItemRepository> logger) : base(client, settings, logger)
        {
        }

        public IDictionary<string,object> ListItems(int limit = 10, int offset = 0)
        {
            AssertListParamsCorrect(limit, offset);
            var request = new RestRequest("/items", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetItemById(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> CreateItem(IDictionary<string, object> item)
        {
            var request = new RestRequest("/items", Method.POST);

            foreach (var key in item.Keys) {
                request.AddParameter(key, item[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public bool DeleteItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}", Method.DELETE);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public IDictionary<string, object> UpdateItem(IDictionary<string, object> item)
        {
            var request = new RestRequest("/items/{id}", Method.PATCH);
            request.AddUrlSegment("id", (string)item["id"]);

            foreach (var key in item.Keys)
            {
                request.AddParameter(key, item[key]);
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string,object> ListTransactionsForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/transactions", Method.GET);
            request.AddUrlSegment("id", itemId);
            IRestResponse response;
            try
            {
                response = SendRequest(Client, request);
            }
            catch (ApiErrorsException e)
            {
                if (e.Errors.Count == 1 && e.Errors.Values.First().First() == "no transaction found")
                {
                    return null;
                }
                throw;
            }
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetStatusForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/status", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ListFeesForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/fees", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetBuyerForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/buyers", Method.GET);
            request.AddUrlSegment("id", itemId);
            IRestResponse response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetSellerForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/sellers", Method.GET);
            request.AddUrlSegment("id", itemId);
            IRestResponse response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetWireDetailsForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/wire_details", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> GetBPayDetailsForItem(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/bpay_details", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ListBatchTransactions(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/batch_transactions", Method.GET);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }


        public IDictionary<string, object> MakePayment(string itemId, string accountId)
        {
            AssertIdNotNull(itemId);
            AssertIdNotNull(accountId);
            var request = new RestRequest("/items/{id}/make_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("account_id", accountId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestPayment(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/request_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ReleasePayment(string itemId, int releaseAmount)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/release_payment", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("release_amount", releaseAmount);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestRelease(string itemId, int releaseAmount)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/request_release", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("release_amount", releaseAmount);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> Cancel(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/cancel", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> AcknowledgeWire(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/acknowledge_wire", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> AcknowledgePayPal(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/acknowledge_paypal", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RevertWire(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/revert_wire", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestRefund(string itemId, string refundAmount, string refundMessage)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/request_refund", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("refund_amount", refundAmount);
            request.AddParameter("refund_message", refundMessage);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> Refund(string itemId, string refundAmount, string refundMessage)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/refund", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("refund_amount", refundAmount);
            request.AddParameter("refund_message", refundMessage);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> DeclineRefund(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/decline_refund", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RaiseDispute(string itemId, string userId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/raise_dispute", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            request.AddParameter("user_id", userId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestDisputeResolution(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/request_resolve_dispute", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> ResolveDispute(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/resolve_dispute", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> EscalateDispute(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/escalate_dispute", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> SendTaxInvoice(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/send_tax_invoice", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string, object> RequestTaxInvoice(string itemId)
        {
            AssertIdNotNull(itemId);
            var request = new RestRequest("/items/{id}/request_tax_invoice", Method.PATCH);
            request.AddUrlSegment("id", itemId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
    }
}
