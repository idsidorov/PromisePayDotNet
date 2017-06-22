using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class CardAccount : AbstractAccount
    {
        [JsonProperty(PropertyName = "card")]
        public Card Card { get; set; }
    }
}
