using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using PromisePayDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicUserTest : AbstractTest
    {
        public string GetUserByIdResponse =
    "{\"created_at\":\"2015-05-18T06:50:51.684Z\",\"updated_at\":\"2015-05-18T11:36:14.050Z\",\"full_name\":\"Igor Sidorov\",\"email\":\"idsidorov@gmail.com\",\"mobile\":null,\"phone\":null,\"first_name\":\"Igor\",\"last_name\":\"Sidorov\",\"id\":\"ef831cd65790e232f6e8c316d6a2ce50\",\"verification_state\":\"pending\",\"held_state\":false,\"dob\":\"Not provided.\",\"government_number\":\"Not provided.\",\"drivers_license\":\"Not provided.\",\"related\":{\"addresses\":\"f08a5f8a-698f-41cf-ac2e-7d5cc52eb915\",\"companies\":\"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\"},\"links\":{\"self\":\"/users/ef831cd65790e232f6e8c316d6a2ce50\",\"items\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/items\",\"card_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/card_accounts\",\"paypal_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/paypal_accounts\",\"bank_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/bank_accounts\"}}";

        [Test]
        public void TestUserDaoObject()
        {
            var user = JsonConvert.DeserializeObject<IDictionary<string,object>>(GetUserByIdResponse);
            Assert.AreEqual("Igor Sidorov", (string)user["full_name"]);
            Assert.IsTrue(((DateTime?)user["created_at"]).HasValue);
            Assert.IsTrue(((DateTime?)user["updated_at"]).HasValue);
        }

        [Test]
        public void UserCreateSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new Dictionary<string,object>
            {
                {"id" , id},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id + "@google.com"},
                {"government_number", "555555555"}
            };

            var resp = repo.CreateUser(user);
            var createdUser = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));

            Assert.AreEqual(user["id"], createdUser["id"]);
            Assert.AreEqual(user["first_name"], createdUser["first_name"]);
            Assert.AreEqual(user["last_name"], createdUser["last_name"]);
            Assert.AreEqual("Test Test", createdUser["full_name"]);
            Assert.AreEqual(user["email"], createdUser["email"]);
            Assert.AreEqual("encrypted", createdUser["government_number"]);
            Assert.IsTrue(((DateTime?)createdUser["created_at"]).HasValue);
            Assert.IsTrue(((DateTime?)createdUser["updated_at"]).HasValue);
        }

        [Test]
        public void ValidationErrorUserCreateMissedId()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new Dictionary<string, object>
              {{"id" , null},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id + "@google.com"}};
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ValidationErrorUserCreateMissedFirstName()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new Dictionary<string, object>
              {{"id" , id},
                {"first_name" , null},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id + "@google.com"}};
              Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ValidationErrorUserCreateWrongCountry()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new Dictionary<string, object>
               {{"id" , null},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "Australia"}, //Incorrect name - not an ISO code
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id + "@google.com"}};
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }
        
        [Test]
        public void ValidationErrorUserCreateWrongEmail()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new Dictionary<string, object>
              {{"id" , null},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id }};
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }
        
        [Test]
        public void ListUsersSuccessful()
        {
            //var id = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            //Then, list users
            var content = File.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var users = repo.ListUsers(200);

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Any());
        }
        
        [Test]
        public void ListUsersNegativeParams()
        {
            var content = File.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.ListUsers(-10, -20));
        }

        [Test]
        public void ListUsersTooHighLimit()
        {
            var content = File.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            Assert.Throws<ArgumentException>(() => repo.ListUsers(201));
        }

        
        [Test]
        public void GetUserSuccessful()
        {
            //First, create a user with known id
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";

            //Then, get user
            var resp = repo.GetUserById(id);
            var gotUser = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.IsNotNull(gotUser);
            Assert.AreEqual(id, (string)gotUser["id"], id);
        }
        
        [Test]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void GetUserMissingId()
        {
            var content = File.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "asdfkjas;lkflaksndflaksndfklas";
            Assert.Throws<ApiErrorsException>(() => repo.GetUserById(id));
        }
        
        [Test]
        [Ignore("Skipped until API method will be fixed")]
        public void DeleteUserSuccessful()
        {
            Assert.Fail();
        }

        
        [Test]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void DeleteUserMissingId()
        {
            var content = File.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, System.Net.HttpStatusCode.NotFound);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = Guid.NewGuid().ToString();
            Assert.IsFalse(repo.DeleteUser(id));
        }

        
        [Test]
        public void EditUserSuccessful()
        {
            //First, create a user we'll work with
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";

            var user = new Dictionary<string, object>
              {{"id" , id},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id  + "@google.com"}};

            var createdUser = repo.CreateUser(user);

            //Now, try to edit newly created user
            user["first_name"] = "Test123";
            user["last_name"] = "Test123";

            content = File.ReadAllText("./Fixtures/user_update.json");
            client = GetMockClient(content);
            repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var resp = repo.UpdateUser(user);
            var modifiedUser = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual("Test123", modifiedUser["first_name"]);
            Assert.AreEqual("Test123", modifiedUser["last_name"]);
            Assert.AreEqual("Test123 Test123", modifiedUser["full_name"]);
        }

        [Test]
        public void EditUserMissingId()
        {
            var content = File.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = Guid.NewGuid().ToString();

            var user = new Dictionary<string, object>
              {{"id" , id},
                {"first_name" , "Test"},
                {"last_name" , "Test"},
                {"city" , "Test"},
                {"address_line1" , "Line 1"},
                {"address_line2" , "Line 2"},
                {"country" , "AUS"},
                {"mobile" , "123123"},
                {"state" , "state"},
                {"zip" , "123456"},
                {"email" , id  + "@google.com"}};

            Assert.Throws<ApiErrorsException>(() => repo.UpdateUser(user));
        }


        [Test]
        [Ignore("Currently, this test returns 401")]
        public void SendMobilePinSuccessful()
        {
            Assert.Fail();
        }
        
        [Test]
        public void ListUserItemsSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_items.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var items = repo.ListItemsForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void SetUserDisbursementAccountSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_set_disbursement_account.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var response = repo.SetDisbursementAccount("5830def0-ffe8-11e5-86aa-5e5517507c66", "46deb476-c1a6-41eb-8eb7-26a695bbe5bc");

            var users = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["users"]));
            var related = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(users["related"]));
            Assert.AreEqual("46deb476-c1a6-41eb-8eb7-26a695bbe5bc", related["payout_account"]);
            
        }

        [Test]
        public void ListUserBankAccountsSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_bank_accounts.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var resp = repo.GetBankAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual("c5d37185-4472-44c1-87e2-8a5a3abb96fc", items["id"]);
        }

        [Test]
        public void ListUserWalletAccountsSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_list_wallet_accounts.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var resp = repo.GetBankAccountForUser("385b50bb-237a-42cb-9382-22953e191ae6");
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp.Values.First()));
            Assert.AreEqual("385b50bb-237a-42cb-9382-22953e191ae6", items["id"]);
        }

        [Test]
        public void GetUserCardAccountSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_card_accounts.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var resp = repo.GetCardAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            var cardAccounts = JsonConvert.DeserializeObject<IDictionary<string,object>>(JsonConvert.SerializeObject(resp["card_accounts"]));
            Assert.AreEqual("81e44baa-b5df-4bcd-a6a7-39e5ecd91a74", cardAccounts["id"]);
        }

        [Test]
        public void GetUserPayPalAccountSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_paypal_accounts.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var resp = repo.GetCardAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            var paypalAccounts = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(resp["paypal_accounts"]));
            Assert.AreEqual("fdc5e5e4-b5d2-456b-8d42-ff349ccf8346", paypalAccounts["id"]);
        }

        [Test]
        public void GetUserBankAccountEmpty()
        {
            var content = File.ReadAllText("./Fixtures/user_bank_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var items = repo.GetBankAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void GetUserCardAccountEmpty()
        {
            var content = File.ReadAllText("./Fixtures/user_card_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var items = repo.GetCardAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void GetUserPayPalAccountEmpty()
        {
            var content = File.ReadAllText("./Fixtures/user_paypal_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var items = repo.GetPayPalAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }
        
        [Test]
        [Ignore("Not implemented yet")]
        public void ListUserDisbursementAccountsEmpty()
        {
            Assert.Fail();
        }
    }
}
