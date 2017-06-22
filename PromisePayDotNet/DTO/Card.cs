using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class Card
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "expiry_month")]
        public string ExpiryMonth { get; set; }

        [JsonProperty(PropertyName = "expiry_year")]
        public string ExpiryYear { get; set; }

        [JsonProperty(PropertyName = "cvv")]
        public string CVV { get; set; }
    }
}
