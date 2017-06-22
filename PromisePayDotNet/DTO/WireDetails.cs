using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class WireDetails
    {
        [JsonProperty(PropertyName = "beneficiary")]
        public string Beneficiary { get; set; }

        [JsonProperty(PropertyName = "address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }

        [JsonProperty(PropertyName = "routing_number")]
        public string RoutingNumber { get; set; }

        [JsonProperty(PropertyName = "account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "bank_name")]
        public string BankName { get; set; }

        [JsonProperty(PropertyName = "swift")]
        public string Swift { get; set; }

        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}
