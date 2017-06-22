using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    internal class DetailsContainer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "bpay_details")]
        public BPayDetails BPayDetails { get; set; }

        [JsonProperty(PropertyName = "wire_details")]
        public WireDetails WireDetails { get; set; }
    }
}
