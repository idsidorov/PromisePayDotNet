using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class User : AbstractDTO
    {
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "address_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public string Dob { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "verification_state")]
        public string VerificationState { get; set; }

        [JsonProperty(PropertyName = "drivers_license")]
        public string DriversLicense { get; set; }
    }
}
