using Newtonsoft.Json;
using System.Collections.Generic;

namespace PromisePayDotNet.Dto
{
    public class Transaction : AbstractDTO
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "related")]
        public IDictionary<string, string> Related { get; set; }
    }
}
