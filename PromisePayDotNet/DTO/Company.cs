using Newtonsoft.Json;
using System.Collections.Generic;

namespace PromisePayDotNet.Dto
{
    public class Company : AbstractDTO
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "legal_name")]
        public string LegalName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "tax_number")]
        public string TaxNumber { get; set; }

        [JsonProperty(PropertyName = "charge_tax")]
        public string ChargeTax { get; set; }

        [JsonProperty(PropertyName = "address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "address_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "related")]
        public IDictionary<string, string> Related { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

    }
}
