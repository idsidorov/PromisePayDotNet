using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public abstract class AbstractAccount : AbstractDTO
    {
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}
