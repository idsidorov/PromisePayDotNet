using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class ItemStatus
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
