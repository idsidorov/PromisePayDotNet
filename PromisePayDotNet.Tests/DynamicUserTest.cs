using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.DTO;

namespace PromisePayDotNet.Tests
{
    public class DynamicUserTest : AbstractTest
    {
        public string GetUserByIdResponse =
    "{\"created_at\":\"2015-05-18T06:50:51.684Z\",\"updated_at\":\"2015-05-18T11:36:14.050Z\",\"full_name\":\"Igor Sidorov\",\"email\":\"idsidorov@gmail.com\",\"mobile\":null,\"phone\":null,\"first_name\":\"Igor\",\"last_name\":\"Sidorov\",\"id\":\"ef831cd65790e232f6e8c316d6a2ce50\",\"verification_state\":\"pending\",\"held_state\":false,\"dob\":\"Not provided.\",\"government_number\":\"Not provided.\",\"drivers_license\":\"Not provided.\",\"related\":{\"addresses\":\"f08a5f8a-698f-41cf-ac2e-7d5cc52eb915\",\"companies\":\"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\"},\"links\":{\"self\":\"/users/ef831cd65790e232f6e8c316d6a2ce50\",\"items\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/items\",\"card_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/card_accounts\",\"paypal_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/paypal_accounts\",\"bank_accounts\":\"/users/ef831cd65790e232f6e8c316d6a2ce50/bank_accounts\"}}";

        [Fact]
        public void TestUserDaoObject()
        {
            var user = JsonConvert.DeserializeObject<IDictionary<string, object>>(GetUserByIdResponse);
            Assert.Equal("Igor Sidorov", (string)user["full_name"]);
            Assert.True(((DateTime?)user["created_at"]).HasValue);
            Assert.True(((DateTime?)user["updated_at"]).HasValue);
        }

        [Fact]
        public void UserCreateSuccessful()
        {
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            var createdUser = repo.CreateUser(user);

            Assert.Equal(user.Id, createdUser.Id);
            Assert.Equal(user.FirstName, createdUser.FirstName);
            Assert.Equal(user.LastName, createdUser.LastName);
            Assert.Equal("Test Test", createdUser.FullName);
            Assert.Equal(user.Email, createdUser.Email);
            Assert.True(createdUser.CreatedAt.HasValue);
            Assert.True(createdUser.UpdatedAt.HasValue);
        }

        [Fact]
        public void ValidationErrorUserCreateMissedId()
        {
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = null,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Fact]
        public void ValidationErrorUserCreateMissedFirstName()
        {
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = id,
                FirstName = null,
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Fact]
        public void ValidationErrorUserCreateWrongCountry()
        {
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = null,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "Australia", //Incorrect name - not an ISO code
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Fact]
        public void ValidationErrorUserCreateWrongEmail()
        {
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";
            var user = new User
            {
                Id = null,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id
            };
            Assert.Throws<ValidationException>(() => repo.CreateUser(user));
        }

        [Fact]
        public void ListUsersSuccessful()
        {
            var id = "ec9bf096-c505-4bef-87f6-18822b9dbf2c";
            //Then, list users
            var content = Files.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);

            var users = repo.ListUsers(200);

            Assert.NotNull(users);
            Assert.True(users.Any());
            Assert.True(users.Any(x => (string)x.Id == id));

        }

        [Fact]
        public void ListUsersNegativeParams()
        {
            var content = Files.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.ListUsers(-10, -20));
        }

        [Fact]
        public void ListUsersTooHighLimit()
        {
            var content = Files.ReadAllText("./Fixtures/users_list.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            Assert.Throws<ArgumentException>(() => repo.ListUsers(201));
        }


        [Fact]
        public void GetUserSuccessful()
        {
            //First, create a user with known id
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";

            //Then, get user
            var gotUser = repo.GetUserById(id);

            Assert.NotNull(gotUser);
            Assert.Equal(id, (string)gotUser.Id);
        }

        [Fact]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void GetUserMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = Get<IUserRepository>(client.Object);
            var id = "asdfkjas;lkflaksndflaksndfklas";
            Assert.Throws<ApiErrorsException>(() => repo.GetUserById(id));
        }

        //[Fact]
        //[Ignore("Skipped until API method will be fixed")]
        public void DeleteUserSuccessful()
        {
            //throw new System.Exception();
        }


        [Fact]
        //That's bad idea not to distinguish between "wrong login/password" and "There is no such ID in DB"
        public void DeleteUserMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, System.Net.HttpStatusCode.NotFound);
            var repo = Get<IUserRepository>(client.Object);
            var id = Guid.NewGuid().ToString();
            Assert.False(repo.DeleteUser(id));
        }


        [Fact]
        public void EditUserSuccessful()
        {
            //First, create a user we'll work with
            var content = Files.ReadAllText("./Fixtures/user_create.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);
            var id = "608ef7e7-6113-4981-a3a3-ab8cea04eb93";

            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            var createdUser = repo.CreateUser(user);

            //Now, try to edit newly created user
            user.FirstName = "Test123";
            user.LastName = "Test123";

            content = Files.ReadAllText("./Fixtures/user_update.json");
            client = GetMockClient(content);
            repo = Get<IUserRepository>(client.Object);
            var modifiedUser = repo.UpdateUser(user);
            Assert.Equal("Test123", modifiedUser.FirstName);
            Assert.Equal("Test123", modifiedUser.LastName);
            Assert.Equal("Test123 Test123", modifiedUser.FullName);
        }

        [Fact]
        public void EditUserMissingId()
        {
            var content = Files.ReadAllText("./Fixtures/user_missing.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = Get<IUserRepository>(client.Object);
            var id = Guid.NewGuid().ToString();

            var user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Country = "AUS",
                Mobile = "123123",
                State = "state",
                Zip = "123456",
                Email = id + "@google.com"
            };

            Assert.Throws<ApiErrorsException>(() => repo.UpdateUser(user));
        }


        //[Fact]
        //[Ignore("Currently, this test returns 401")]
        public void SendMobilePinSuccessful()
        {
            throw new System.Exception();
        }

        [Fact]
        public void ListUserItemsSuccessful()
        {
            var content = Files.ReadAllText("./Fixtures/user_items.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.ListItemsForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        //[Fact]
        //[Ignore("Not implemented yet")]
        public void ListUserDisbursementAccountsSuccessful()
        {
            throw new System.Exception();
        }

        [Fact]
        public void ListUserBankAccountsSuccessful()
        {
            var content = Files.ReadAllText("./Fixtures/user_bank_accounts.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetBankAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            Assert.Equal("c5d37185-4472-44c1-87e2-8a5a3abb96fc", items.Id);
        }

        [Fact]
        public void GetUserCardAccountSuccessful()
        {
            var content = Files.ReadAllText("./Fixtures/user_card_accounts.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetCardAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            Assert.Equal("81e44baa-b5df-4bcd-a6a7-39e5ecd91a74", items.Id);
        }

        [Fact]
        public void GetUserPayPalAccountSuccessful()
        {
            var content = Files.ReadAllText("./Fixtures/user_paypal_accounts.json");
            var client = GetMockClient(content);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetPayPalAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
            Assert.Equal("fdc5e5e4-b5d2-456b-8d42-ff349ccf8346", items.Id);
        }

        [Fact]
        public void GetUserBankAccountEmpty()
        {
            var content = Files.ReadAllText("./Fixtures/user_bank_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetBankAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Fact]
        public void GetUserCardAccountEmpty()
        {
            var content = Files.ReadAllText("./Fixtures/user_card_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetCardAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        [Fact]
        public void GetUserPayPalAccountEmpty()
        {
            var content = Files.ReadAllText("./Fixtures/user_paypal_accounts_empty.json");
            var client = GetMockClient(content, (System.Net.HttpStatusCode)422);
            var repo = Get<IUserRepository>(client.Object);

            var items = repo.GetPayPalAccountForUser("89592d8a-6cdb-4857-a90d-b41fc817d639");
        }

        //[Fact]
        //[Ignore("Not implemented yet")]
        public void ListUserDisbursementAccountsEmpty()
        {
            //throw new System.Exception();
        }
    }
}
