using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class BPayDetails
    {
        [JsonProperty(PropertyName = "biller_code")]
        public string BillerCode { get; set; }

        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
    }
}
