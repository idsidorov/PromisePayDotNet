using Newtonsoft.Json;
using PromisePayDotNet.Enums;

namespace PromisePayDotNet.Dto
{
    public class Token //NOTE: Why is this called token?
    {
        [JsonProperty(PropertyName = "current_user_id")]
        public string CurrentUserId { get; set; }

        [JsonProperty(PropertyName = "current_user")]
        public string CurrentUser { get; set; }

        [JsonProperty(PropertyName = "item_name")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "seller_lastname")]
        public string SellerLastName { get; set; }

        [JsonProperty(PropertyName = "seller_firstname")]
        public string SellerFirstName { get; set; }

        [JsonProperty(PropertyName = "seller_country")]
        public string SellerCountry { get; set; }

        [JsonProperty(PropertyName = "buyer_lastname")]
        public string BuyerLastName { get; set; }

        [JsonProperty(PropertyName = "buyer_firstname")]
        public string BuyerFirstName { get; set; }

        [JsonProperty(PropertyName = "buyer_country")]
        public string BuyerCountry { get; set; }

        [JsonProperty(PropertyName = "seller_email")]
        public string SellerEmail { get; set; }

        [JsonProperty(PropertyName = "buyer_email")]
        public string BuyerEmail { get; set; }

        [JsonProperty(PropertyName = "external_item_id")]
        public string ExternalItemId { get; set; }

        [JsonProperty(PropertyName = "external_seller_id")]
        public string ExternalSellerId { get; set; }

        [JsonProperty(PropertyName = "external_buyer_id")]
        public string ExternalBuyerId { get; set; }

        [JsonProperty(PropertyName = "fee_ids")]
        public string FeeIds { get; set; }

        [JsonProperty(PropertyName = "payment_type_id")]
        public PaymentType PaymentType { get; set; }
   }
}
