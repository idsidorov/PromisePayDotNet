# .NET SDK - PromisePay API

[![Join the chat at https://gitter.im/PromisePay/promisepay-dotnet](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/PromisePay/promisepay-dotnet?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![NuGet version](https://badge.fury.io/nu/PromisePay.API.NET.svg)](https://badge.fury.io/nu/PromisePay.API.NET) [![Build Status](https://travis-ci.org/PromisePay/promisepay-dotnet.svg)](https://travis-ci.org/PromisePay/promisepay-dotnet) [![Code Climate](https://codeclimate.com/github/PromisePay/promisepay-dotnet/badges/gpa.svg)](https://codeclimate.com/github/PromisePay/promisepay-dotnet) 

# 1. Installation
**NuGet:** Install PromisePay via NuGet package manager. The package name is '[PromisePay.API.NET](https://www.nuget.org/packages/PromisePay.API.NET)'.

**Source:** Download latest sources from GitHub, add project into your solution and build it.

# 2. Configuration

Before interacting with PromisePay API, you'll need to [create a prelive account](https://management.prelive.promisepay.com/#/sign-up/prelive) and get an API key.

Once you have recorded your API token, configure the .NET package - see below.

Add the below configuration (or equivalent) to your appsettings.json file.
```javascript
{
	"AssemblyPay": {
		"Url": "https://test.api.promisepay.com",
		"Login": "YOUR_USERNAME",
		"Password": "YOUR_ACCOUNT_KEY"
	}
}
```
Then, in your Startup.cs, when you are setting up your Service Collection:
```cs
using Microsoft.Extensions.Configuration;
...
// FOR OLD STYLE REPOSITORIES
serviceCollection.AddAssemblyPay(config.GetSection("AssemblyPay").Get<Settings>())
// OR FOR NEW DYNAMIC REPOSITORIES
serviceCollection.AddAssemblyPayDynamic(config.GetSection("AssemblyPay").Get<Settings>())
```
As .NET Core configuration is quite flexible, the configuration could be loaded a number of ways, and the above is meant as a sample only.


**TLS 1.2**
We require a minimum of TLS 1.2 on the client connection. 

**Environments**

	Prelive: https://test.api.promisepay.com
	Production: https://secure.api.promisepay.com

**Final configuration**

PromisePay API package is built using Dependency Injection. It makes integration into your application easy and seamless.

By calling the `.AddAssemblyPay` or `.AddAssemblyPayDynamic` extension method, you are adding the relevant PromisePay services into your service configuration DI container.


If you use another container, please refer to the code in PromisePayDotNet\Extensions\ServiceCollectionExtensions.cs file to create your own implementation.
You may use any lifecycle; implementations are stateless.


Then, you can use repositories from package, by resolving interface with container, or passing dependencies into constructor.

# 3. Examples
## Tokens
##### Example 1 - Request session token
The below example shows the request for a marketplace configured to have the Item and User IDs generated automatically for them.

```cs
var repo = serviceProvider.GetService<ITokenRepository>();
var session_token = new Dictionary<string,object> {
	{"current_user" , "seller"},
	{"item_name" , "Test Item"},
	{"amount" , "2500"},
	{"seller_lastname" , "Seller"},
	{"seller_firstname" , "Sally"},
	{"buyer_lastname" , "Buyer"},
	{"buyer_firstname" , "Bobby"},
	{"buyer_country" , "AUS"},
	{"seller_country" , "USA"},
	{"seller_email" , "sally.seller@promisepay.com"},
	{"buyer_email" , "bobby.buyer@promisepay.com"},
	{"fee_ids" , ""},
	{"payment_type_id" , "2"}
};
```
##### Example 2 - Request session token
The below example shows the request for a marketplace that passes the Item and User IDs.

```cs
var repo = serviceProvider.GetService<ITokenRepository>();
var session_token = new Dictionary<string, object> {
	{"current_user_id", "seller1234"},
	{"item_name", "Test Item"},
	{"amount", "2500"},
	{"seller_lastname", "Seller"},
	{"seller_firstname", "Sally"},
	{"buyer_lastname", "Buyer"},
	{"buyer_firstname", "Bobby"},
	{"buyer_country", "AUS"},
	{"seller_country", "USA"},
	{"seller_email", "sally.seller@promisepay.com"},
	{"buyer_email", "bobby.buyer@promisepay.com"},
	{"external_item_id", "TestItemId1234"},
	{"external_seller_id", "seller1234"},
	{"external_buyer_id", "buyer1234"},
	{"fee_ids", = ""},
	{"payment_type_id = "2"}		
};
```

##### Generate a card token
```cs
	var repo = serviceProvider.GetService<ITokenRepository>();
    var token = repo.GenerateCardToken("card", "064d6800-fff3-11e5-86aa-5e5517507c66");
```

## Addresses

##### Get Address By Id
```cs
	var repo = serviceProvider.GetService<IAddressRepository>();
    var resp = repo.GetAddressById("07ed45e5-bb9d-459f-bb7b-a02ecb38f443");
```

## Batch transactions

##### List Batch Transactions
```cs
	var repo = serviceProvider.GetService<IBatchTransactionRepository>();
    var response = repo.List();
	var transactions = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(response["batch_transactions"]));
```

##### Show Batch Transaction
```cs
	var repo = serviceProvider.GetService<IBatchTransactionRepository>();
    const string id = "b1652611-9544-4244-a601-54c24cfa5e90";
    var response = repo.Show(id);
    var transaction = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["batch_transactions"]));
```

## Items

##### Create an item
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = Guid.NewGuid().ToString();
var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
var item = new Dictionary<string,object>
{
	{"id", id},
	{"name", "Test Item #1"},
	{"amount", 1000},
	{"payment_type_id", (int)PaymentType.Express},
	{"buyer_id", buyerId}, //optional field
	{"seller_id", sellerId}, //optional field
	//No fee at this stage, optional field
	{"description", "Test item #1 description"}
};
var createdItem = repo.CreateItem(item);
```

##### Get an item
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "36aa17fb-5ea6-432b-8363-8074ae02603d";
var gotItem = repo.GetItemById(id);
```

##### Get a list of items
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var items = repo.ListItems();
```

##### Update an item
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var buyerId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
var sellerId = "fdf58725-96bd-4bf8-b5e6-9b61be20662e"; //some user created before
var item = new Dictionary<string,object>
{
	{"id", id},
	{"name", "Test Item #1"},
	{"amount", 1000},
	{"payment_type_id", (int)PaymentType.Express},
	{"buyer_id", buyerId}, //optional field
	{"seller_id", sellerId}, //optional field
	//No fee at this stage, optional field
	{"description", "Test item #1 description"}
};

var updatedItem = repo.UpdateItem(item);

```
##### Delete an item
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var result = repo.DeleteItem(id);
```

##### List Item Batch Transactions
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var response = repo.ListBatchTransactions("7c269f52-2236-4aa5-899e-a2e3ecadbc3f");
```

##### Get an item status
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var status = repo.GetStatusForItem(id);
```

##### Get an item's buyer
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var buyer = repo.GetBuyerForItem(id);
```

##### Get an item's seller
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var seller = repo.GetSellerForItem(id);
```

##### Get an item's fees
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var fees = repo.ListFeesForItem(id);
```

##### Get an item's transactions
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var transactions = repo.ListTransactionsForItem(id);
```
##### Get an item's wire details
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var wireDetails = repo.GetWireDetailsForItem(id);
```

##### Get an item's BPAY details
```cs
var repo = serviceProvider.GetService<IItemRepository>();
var id = "bb2323cf-4838-4fcb-a288-933d0307523d";
var bPayDetails = repo.GetBPayDetailsForItem(id);
```

## Direct debit authority

##### Create Direct Debit Authority
```cs
var repo = serviceProvider.GetService<IDirectDebitAuthorityRepository>();
var resp = repo.Create("9fda18e7-b1d3-4a83-830d-0cef0f62cd25", "100000");
```

##### List Direct Debit Authorities
```cs
var repo = serviceProvider.GetService<IDirectDebitAuthorityRepository>();
var resp = repo.List("9fda18e7-b1d3-4a83-830d-0cef0f62cd25");
```

##### Show Direct Debit Authority
```cs
var repo = serviceProvider.GetService<IDirectDebitAuthorityRepository>();
var resp = repo.Show("8f233e04-ffaa-4c9d-adf9-244853848e21");
```

##### Delete Direct Debit Authority
```cs
var repo = serviceProvider.GetService<IDirectDebitAuthorityRepository>();
var resp = repo.Delete("9fda18e7-b1d3-4a83-830d-0cef0f62cd25");
```

## Users

##### Create a user

```cs
var repo = serviceProvider.GetService<IUserRepository>();

var id = Guid.NewGuid().ToString();
var user = new Dictionary<string, object>
{
    {"id", id},
    {"first_name", "Test"},
    {"last_name", "Test"},
    {"city = "Test"},
    {"address_line1" = "Line 1"},
    {"country" = "AUS"},
    {"state" = "state"},
    {"zip" = "123456"},
    {"email" = id + "@google.com"}
};

var createdUser = repo.CreateUser(user);	
```

##### Get a user

```cs
var repo = serviceProvider.GetService<IUserRepository>();
var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";

var user = repo.GetUserById(id);
```

##### Get a list of users

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var users = repo.ListUsers();
```

##### Delete a User

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	repo.DeleteUser(id);
```

##### Get a user's card account

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var account = repo.GetCardAccountForUser(id);	
```

##### Get a user's PayPal account

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var account = repo.GetPayPalAccountForUser(id);
```

##### Get a user's bank account

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var account = repo.GetBankAccountForUser(id);	
```

##### Get a user's items

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var id = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var items = repo.ListItemsForUser(id);
```

##### Set a user's disbursement account

```cs
	var repo = serviceProvider.GetService<IUserRepository>();
	var userId = "871f83ce-c55d-43ce-ba97-c65628d041a9";	
	var accountId = "d077620f-f207-451c-abea-9ed430ea2cbf";	
	bool result = repo.SetDisbursementAccount(userId, accountId);
```

## Item Actions
##### Make payment
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var accountId = "d077620f-f207-451c-abea-9ed430ea2cbf";
	var item = repo.MakePayment(itemId, accountId);
```

##### Request payment
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var item = repo.RequestPayment(itemId);
```

##### Release payment
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var releaseAmount = 123;
	var item = repo.ReleasePayment(itemId, releaseAmount);
```

##### Request release
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var releaseAmount = 123;
	var item = repo.RequestRelease(itemId, releaseAmount);
```

##### Cancel
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var item = repo.Cancel(itemId);
```

##### Acknowledge wire
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var item = repo.AcknowledgeWire(itemId);
```

##### Acknowledge PayPal
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var item = repo.AcknowledgePayPal(itemId);
```

##### Revert wire
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var item = repo.RevertWire(itemId);
```

##### Request refund
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var refundAmount = 123;
	var refundMessage = "refund message";
	var item = repo.RequestRefund(itemId, refundAmount, refundMessage);
```

##### Refund
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "871f83ce-c55d-43ce-ba97-c65628d041a9";
	var refundAmount = 123;
	var refundMessage = "refund message";
	var item = repo.Refund(itemId, refundAmount, refundMessage);
```

##### Decline Refund
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
    var response = repo.DeclineRefund(itemId);
```

##### Raise Dispute
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
	var userId = "5830def0-ffe8-11e5-86aa-5e5517507c66";
    var response = repo.RaiseDispute(itemId, userId);
```

##### Request Dispute Resolution
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
    var response = repo.RequestDisputeResolution(itemId);
```

##### Resolve Dispute
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
	var response = repo.ResolveDispute(itemId);
```

##### Escalate Dispute
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
    var response = repo.EscalateDispute(itemId);
```

##### Send Tax Invoice
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
    var response = repo.SendTaxInvoice(itemId);
```

##### Request Tax Invoice
```cs
	var repo = serviceProvider.GetService<IItemRepository>();
	var itemId = "100fd4a0-0538-11e6-b512-3e1d05defe78";
    var response = repo.RequestTaxInvoice(itemId);
```

## Card Accounts
##### Create a card account
```cs
var repo = serviceProvider.GetService<ICardAccountRepository>();
var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
var account = new Dictionary<string,object>
{
	{"user_id", userId},
	{"active = true,
	{"card = new Dictionary<string,object>
	{
		{"full_name", "Batman"},
		{"expiry_month", "11"},
		{"expiry_year", "2020"},
		{"number", "4111111111111111"},
		{"type", "visa"},
		{"cvv", "123"}
	}
};
var createdAccount = repo.CreateCardAccount(account);
var id = createdAccount.Id;
```

##### Get a card account
```cs
var repo = serviceProvider.GetService<ICardAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotAccount = repo.GetCardAccountById(accountId);
```

##### Delete a card account
```cs
var repo = serviceProvider.GetService<ICardAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var result = repo.DeleteCardAccount(accountId); //result should be true
var gotAccount = repo.GetCardAccountById(accountId); //gotAccount.Active should be false
```

##### Get a card account's users
```cs
var repo = serviceProvider.GetService<ICardAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotUser = repo.GetUserForCardAccount(accountId);
```

## Bank Accounts
##### Create a bank account
```cs
var repo = serviceProvider.GetService<IBankAccountRepository>();
var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
var account = new Dictionary<string,object>
{
	{"user_id", userId},
	{"active", true},
	{"bank", new Dictionary<string,object>
	{
		{"bank_name", "Test bank, inc"},
		{"accountName", "Test account"},
		{"accountNumber", "8123456789"},
		{"accountType", "savings"},
		{"country", "AUS"},
		{"holderType", "personal"},
		{"routingNumber", "123456"}
	}
};
var createdAccount = repo.CreateBankAccount(account);
```

##### Get a bank account
```cs
var repo = serviceProvider.GetService<IBankAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotAccount = repo.GetBankAccountById(accountId);
```

##### Delete a bank account
```cs
var repo = serviceProvider.GetService<IBankAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var result = repo.DeleteBankAccount(accountId); //result should be true
var gotAccount = repo.GetBankAccountById(accountId); //gotAccount.Active should be false
```

##### Get a bank account's users
```cs
var repo = serviceProvider.GetService<IBankAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotUser = repo.GetUserForBankAccount(accountId);
```

##### Validate routing number
```cs
var repo = serviceProvider.GetService<IBankAccountRepository>();
var resp = repo.ValidateRoutingNumber("122235821");
```

## PayPal Accounts
##### Create a PayPal account
```cs
var repo = serviceProvider.GetService<IPayPalAccountRepository>();
var userId = "ec9bf096-c505-4bef-87f6-18822b9dbf2c"; //some user created before
var account = new Dictionary<string,object>
{
	{"user_id", userId},
	{"active", true},
	{"paypal", new Dictionary<string,object>
	{
		{"email", "aaa@bbb.com"}
	}
};
var createdAccount = repo.CreatePayPalAccount(account);
```
##### Get a PayPal account
```cs
var repo = serviceProvider.GetService<IPayPalAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotAccount = repo.GetPayPalAccountById(accountId);
```
##### Delete a PayPal account
```cs
var repo = serviceProvider.GetService<IPayPalAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var result = repo.DeletePayPalAccount(accountId); //result should be true
var gotAccount = repo.GetPayPalAccountById(accountId); //gotAccount.Active should be false
```

##### Get a PayPal account's users
```cs
var repo = serviceProvider.GetService<IPayPalAccountRepository>();
var accountId = "14a74a3c-8358-4c99-bcf2-4c6ed7454747";
var gotUser = repo.GetUserForPayPalAccount(accountId);
```

## Fees
##### Get a list of fees
```cs
var repo = serviceProvider.GetService<IFeeRepository>();
var fees = repo.ListFees();
```

##### Get a fee
```cs
var repo = serviceProvider.GetService<IFeeRepository>();
var id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
var fee = repo.GetFeeById(id);
```

##### Create a fee
```cs
var repo = serviceProvider.GetService<IFeeRepository>();
var feeId = Guid.NewGuid().ToString();
var createdFee = repo.CreateFee(new Dictionary<string,object>
{
	{"id", feeId},
	{"amount", 1000},
	{"name", "Test fee #1"},
	{"fee_type_id", (int)FeeType.Fixed},
	{"cap", "1"},
	{"max", "3"},
	{"min", "2"},
	{"to", "buyer"}
});
```

## Transactions
##### Get a list of transactions
```cs
var repo = serviceProvider.GetService<ITransactionRepository>();
var transactions = repo.ListTransactions();
```

##### Get a transactions
```cs
var repo = serviceProvider.GetService<ITransactionRepository>();
var id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
var transaction = repo.GetTransaction(id);
```

##### Get a transaction's users
```cs
var repo = serviceProvider.GetService<ITransactionRepository>();
var id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
var user = repo.GetUserForTransaction(id);
```

##### Get a transaction's fees
```cs
var repo = serviceProvider.GetService<ITransactionRepository>();
var id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
var fee = repo.GetFeeForTransaction(id);
```

## Charges
##### Create charge
```cs
var repo = serviceProvider.GetService<IChargeRepository>();
var charge = new Dictionary<string, object>
{
    {"name" , "Charge for Delivery"},
    {"account_id" , "b49d943f-add0-4d1c-b357-0f1a8fde677c"},
    {"amount" , "4500"},
    {"email" , "abc@abc.com"},
    {"zip" , "3000"},
    {"country" , "AUS"},
    {"user_id" , "7af96d61-2339-4298-8a09-aadd6c4501b2"},
    {"fee_ids" , "187"},
    {"currency" , "AUD"},
    {"retain_account" , "false"},
    {"device_id" , "123456"},
    {"ip_address" , "127.0.0.1"}
};

var response = repo.CreateCharge(charge);
```

##### List charges
```cs
var repo = serviceProvider.GetService<IChargeRepository>();
var charges = repo.ListCharges();
```

##### Show charge
```cs
var repo = serviceProvider.GetService<IChargeRepository>();
var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";
var response = repo.ShowCharge(id);
var charge = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["charges"]));
```

##### Show buyer for a charge
```cs
var repo = serviceProvider.GetService<IChargeRepository>();
var id = "cb7eafc1-571c-425c-9adc-f56cb585cd68";
var response = repo.ShowChargeBuyer(id);
var buyer = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["users"]));
```

##### Show charge status
```cs
var repo = serviceProvider.GetService<IChargeRepository>();
var response = repo.ShowChargeStatus(id);
var charge = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["charges"]));
```

## Feature Configuration
##### Create
```cs
var repo = serviceProvider.GetService<IConfigurationRepository>();
var response = repo.Create(new Dictionary<string,object> {{"name","test"}});
...
```
##### List
```cs
var repo = serviceProvider.GetService<IConfigurationRepository>();
var response = repo.List();
...
```
##### Show
```cs
var repo = serviceProvider.GetService<IConfigurationRepository>();
var response = repo.Show("ca321b3f-db87-4d75-ba05-531c7f1bb515");
...
```
##### Update
```cs
var repo = serviceProvider.GetService<IConfigurationRepository>();
var response = repo.Update(new Dictionary<string,object> {{"id",""ca321b3f-db87-4d75-ba05-531c7f1bb515""}, {"name","test"}});
...
```
##### Delete
```cs
var repo = serviceProvider.GetService<IConfigurationRepository>();
var response = repo.Delete("ca321b3f-db87-4d75-ba05-531c7f1bb515");
...
```

## Restrictions
##### List
```cs
var repo = serviceProvider.GetService<IRestrictionRepository>();
var response = repo.List();
...
```
##### Show
```cs
var repo = serviceProvider.GetService<IRestrictionRepository>();
var response = repo.Show("ca321b3f-db87-4d75-ba05-531c7f1bb515");
...
```

## Tools
##### Health check
```cs
var repo = serviceProvider.GetService<IToolRepository>();
var response = repo.HealthCheck();
if (response["status"] == 'healthy') 
...
```

# 4. Contributing
	1. Fork it ( https://github.com/PromisePay/promisepay-dotnet/fork )
	2. Create your feature branch (`git checkout -b my-new-feature`)
	3. Commit your changes (`git commit -am 'Add some feature'`)
	4. Push to the branch (`git push origin my-new-feature`)
	5. Create a new Pull Request
