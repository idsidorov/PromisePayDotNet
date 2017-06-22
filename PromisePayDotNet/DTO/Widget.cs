using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class Widget
    {
        [JsonProperty(PropertyName = "item_name")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "full_amount")]
        public string FullAmount { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "fees")]
        public string Fees { get; set; }

        [JsonProperty(PropertyName = "fee_name")]
        public string FeeName { get; set; }

        [JsonProperty(PropertyName = "remaining_amount")]
        public string RemainingAmount { get; set; }

        [JsonProperty(PropertyName = "release_request_amount")]
        public string ReleaseRequestAccount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "action_name")]
        public string ActionName { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "has_pending_release")]
        public bool HasPendingRelease { get; set; }

        [JsonProperty(PropertyName = "verification_state")]
        public string VerificationState { get; set; }

        [JsonProperty(PropertyName = "verification_information")]
        public bool VerificationInformation { get; set; }

        [JsonProperty(PropertyName = "disbursement_account")]
        public bool DisbursementAccount { get; set; }

        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "dispute_user")]
        public string DisputeUser { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "other_user_name")]
        public string OtherUserName { get; set; }

        [JsonProperty(PropertyName = "primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty(PropertyName = "secondary_color")]
        public string SecondaryColor { get; set; }

        [JsonProperty(PropertyName = "third_color")]
        public string ThirdColor { get; set; }

        [JsonProperty(PropertyName = "fourth_color")]
        public string FourthColor { get; set; }

        [JsonProperty(PropertyName = "bank_account")]
        public bool BankAccount { get; set; }

        [JsonProperty(PropertyName = "tax_invoice")]
        public string TaxInvoice { get; set; }

        [JsonProperty(PropertyName = "buyer_url")]
        public string BuyerUrl { get; set; }

        [JsonProperty(PropertyName = "seller_url")]
        public string SellerUrl { get; set; }
    }
}
