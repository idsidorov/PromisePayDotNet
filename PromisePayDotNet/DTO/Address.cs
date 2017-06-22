using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class Address : AbstractDTO
    {
        [JsonProperty(PropertyName = "addressline1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressline2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}
