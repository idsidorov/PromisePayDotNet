using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.DTO;

namespace PromisePayDotNet.Tests
{
    public class WidgetTest
    {
        [Fact]
        public void WidgetDeserialization()
        {
            const string jsonStr = "{ item_name: \"Toyota Hilux\", full_amount: \"$1,455.00\", amount: \"$1,500.00\", fees: \"$45.00\", fee_name: \"Seller Fee @ 3%\", remaining_amount: \"$1,500.00\", release_request_amount: null, currency: \"USD\", description: \"<strong>Payment has not yet been made.</strong><br/>Bobby Buyer must place the above amount into escrow to begin this transaction. Their payment will be locked in our vault until you have delivered 'Toyota Hilux'.\", action_name: \"Request payment\", status: \"pending\", has_pending_release: true, verification_state: \"pending\", verification_information: false, disbursement_account: false, payment_method: \"bank\", dispute_user: null, role: \"seller\", other_user_name: \"Bobby Buyer\", primary_color: null, secondary_color: null, third_color: null, fourth_color: null, bank_account: false, tax_invoice: null, buyer_url: \"C:/promisepay-marketplace/app/index.html\", seller_url: \"C:/promisepay-marketplace/app/index.html\" }";

            var widget = JsonConvert.DeserializeObject<Widget>(jsonStr);
            Assert.Equal("Toyota Hilux", widget.ItemName);
            Assert.Equal("bank", widget.PaymentMethod);
        }
    }
}
