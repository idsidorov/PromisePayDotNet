using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Implementations;
using System;
using System.Linq;
using RestSharp;
using System.IO;

namespace PromisePayDotNet.Tests
{
    public class UserTest : AbstractTest
    {
        public string GetUserByIdResponse =
            "{\"created_at\":\"2015-05-18T06:50:51.684Z\",\"updated_at\":\"2015-05-18T11:36:14.050Z\",\"full_name\":\"Igor Sidorov\",\"email\":\"idsidorov@gmail.com\",\"mobile\":null,\"phone\":null,\"first_name\":\"Igor\",\"last_name\":\"Sidorov\",\"id\":\"ef831cd65790e232f6e8c316d6a2ce50\",\"verification_state\":\"pending\",\"held_state\":false,\"dob\":\"Not provided.\",\"government_number\":\"Not provided.\",\"drivers_license\":\"Not provided.\",\"related\":{\"addresses\":\"f08a5f8a-698f-41cf-ac2e-7d5cc52eb915\",\"companies\":\"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\"},\"links\":{\"self\":\"/users/ef831cd65790e232f6e8c316d6a2ce50\",\"items\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/items\",\"card_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/card_accounts\",\"paypal_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/paypal_accounts\",\"bank_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/bank_accounts\"}}";

        [Test]
        public void TestUserDaoObject()
        {
            var user = JsonConvert.DeserializeObject<User>(GetUserByIdResponse);
            Assert.AreEqual("Igor Sidorov", user.FullName);
            Assert.IsTrue(user.CreatedAt.HasValue);
            Assert.IsTrue(user.UpdatedAt.HasValue);
        }

        [Test]
        public void UserCreateSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com",
                DriversLicense = "123123",
                GovernmentNumber = "123123"
            };

            var createdUser = repo.CreateUser(user);

            Assert.AreEqual(user.Id, createdUser.Id);
            Assert.AreEqual(user.FirstName, createdUser.FirstName);
            Assert.AreEqual(user.LastName, createdUser.LastName);
            Assert.AreEqual("Test Test", createdUser.FullName);
            Assert.AreEqual(user.Email, createdUser.Email);
            Assert.AreEqual(user.DriversLicense, createdUser.DriversLicense);
            Assert.AreEqual("encrypted", createdUser.GovernmentNumber);
            Assert.IsTrue(createdUser.CreatedAt.HasValue);
            Assert.IsTrue(createdUser.UpdatedAt.HasValue);
        }

        [Test]
        public void ValidationErrorUserCreateMissedId()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = null,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ValidationErrorUserCreateMissedFirstName()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = null,
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ValidationErrorUserCreateWrongCountry()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "Australia", //Not a correct ISO code
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ValidationErrorUserCreateWrongEmail()
        {
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Test]
        public void ListUsersSuccessful()
        {
            //First, create a user, so we'll have at least one 
            var content = File.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            var createdUser = repo.CreateUser(user);

            //Then, list users
            content = File.ReadAllText("./Fixtures/users_list.json");
            client = GetMockClient(content);
            repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var users = repo.ListUsers(200);

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Any());
            Assert.IsTrue(users.Any(x => x.Id == id));

        }

        [Test]
        public void ListUsersNegativeParams()
        {
            var content = File.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            Assert.Throws<ArgumentException>(()=>repo.ListUsers(-10, -20));
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
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            var createdUser = repo.CreateUser(user);

            //Then, get user
            var gotUser = repo.GetUserById(id);

            Assert.IsNotNull(gotUser);
            Assert.AreEqual(gotUser.Id, id);
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
            //First, create a user with known id
            var repo = new UserRepository(new RestClient(), GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            var createdUser = repo.CreateUser(user);

            //Then, get user
            var gotUser = repo.GetUserById(id);
            Assert.IsNotNull(gotUser);
            Assert.AreEqual(gotUser.Id, id);

            //Now, delete user
            repo.DeleteUser(id);

            //And check whether user exists now
            var success = false;
            try
            {
                repo.GetUserById(id);
            }
            catch (UnauthorizedException)
            {
                success = true;
            }

            if (!success)
            {
                Assert.Fail("Delete user failed!");
            }
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
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com",
                DriversLicense = "123123",
                GovernmentNumber = "123123"
            };

            var createdUser = repo.CreateUser(user);

            //Now, try to edit newly created user
            user.FirstName = "Test123";
            user.LastName = "Test123";

            content = File.ReadAllText("./Fixtures/user_update.json");
            client = GetMockClient(content);
            repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
            var modifiedUser = repo.UpdateUser(user);
            Assert.AreEqual("Test123", modifiedUser.FirstName);
            Assert.AreEqual("Test123", modifiedUser.LastName);
            Assert.AreEqual("Test123 Test123", modifiedUser.FullName);
            Assert.AreEqual(user.DriversLicense, modifiedUser.DriversLicense);
            Assert.AreEqual("encrypted", modifiedUser.GovernmentNumber);
        }

        [Test]
        public void EditUserMissingId()
        {
            var content = File.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object); 
            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                Country = "AUS",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            Assert.Throws<ApiErrorsException>(() => repo.UpdateUser(user));
        }


        [Test]
        [Ignore("Currently, this test returns 401")] 
        public void SendMobilePinSuccessful()
        {
            var repo = new UserRepository(new RestClient(), GetMockSettings().Object, GetMockLogger<UserRepository>().Object);
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
        public void ListUserBankAccountsEmpty() 
        {
            var content = File.ReadAllText("./Fixtures/user_bank_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object); 

            var items = repo.ListBankAccountsForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void ListUserCardAccountsEmpty()
        {
            var content = File.ReadAllText("./Fixtures/user_card_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object); 

            var items = repo.ListCardAccountsForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void ListUserPayPalAccountsEmpty()
        {
            var content = File.ReadAllText("./Fixtures/user_paypal_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object); 

            var items = repo.ListPayPalAccountsForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Test]
        public void SetDisbursementAccountsSuccessful()
        {
            var content = File.ReadAllText("./Fixtures/user_set_disbursement_account.json");
            var client = GetMockClient(content);
            var repo = new UserRepository(client.Object, GetMockSettings().Object, GetMockLogger<UserRepository>().Object);

            var account = repo.SetDisbursementAccount("ec9bf096-c505-4bef-87f6-18822b9dbf2c", "09b5bb3c-c0fd-404d-b373-d675f42d8865");

            Assert.AreEqual("Samuel Seller", account.FullName);
            Assert.AreEqual("approved", account.VerificationState);

        }
    }
}
