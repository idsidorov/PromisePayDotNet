using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class BankAccount : AbstractAccount
    {
        [JsonProperty(PropertyName = "bank")]
        public Bank Bank { get; set; }
    }
}
