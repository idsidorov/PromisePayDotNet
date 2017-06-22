using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class PayPal
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
