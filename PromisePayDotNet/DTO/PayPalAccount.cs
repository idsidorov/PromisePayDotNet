using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class PayPalAccount : AbstractAccount
    {
        [JsonProperty(PropertyName = "paypal")]
        public PayPal PayPal { get; set; }
    }
}
