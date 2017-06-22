using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class Bank
    {
        [JsonProperty(PropertyName = "bank_name")]
        public string BankName { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "account_name")]
        public string AccountName { get; set; }

        [JsonProperty(PropertyName = "routing_number")]
        public string RoutingNumber { get; set; }

        [JsonProperty(PropertyName = "account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "holder_type")]
        public string HolderType { get; set; }

        [JsonProperty(PropertyName = "account_type")]
        public string AccountType { get; set; }
    }
}
