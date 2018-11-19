using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Implementations
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        public UserRepository(IRestClient client, ISettings settings, ILogger<UserRepository> logger) : base(client, settings, logger)
        {
        }

        #region public methods

        public IEnumerable<User> ListUsers(int limit = 10, int offset = 0)
        {
            AssertListParamsCorrect(limit, offset);
            var request = new RestRequest("/users", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var userCollection = dict["users"];
                return JsonConvert.DeserializeObject<List<User>>(JsonConvert.SerializeObject(userCollection));
            }
            return new List<User>();
        }

        public User GetUserById(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}", Method.GET);
            request.AddUrlSegment("id", userId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, User>>(response.Content).Values.First();
        }

        public User CreateUser(User user)
        {
            ValidateUser(user);
            var request = new RestRequest("/users", Method.POST);
            request.AddParameter("id", user.Id);
            request.AddParameter("first_name", user.FirstName);
            request.AddParameter("last_name", user.LastName);
            request.AddParameter("email", user.Email);
            request.AddParameter("mobile", user.Mobile);
            request.AddParameter("address_line1", user.AddressLine1);
            request.AddParameter("address_line2", user.AddressLine2);
            request.AddParameter("state", user.State);
            request.AddParameter("city", user.City);
            request.AddParameter("zip", user.Zip);
            request.AddParameter("country", user.Country);

            request.AddParameter("drivers_license", user.DriversLicense);
            request.AddParameter("government_number", user.GovernmentNumber);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string,User>>(response.Content).Values.First();
        }

        public bool DeleteUser(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}", Method.DELETE);
            request.AddUrlSegment("id", userId);
            var response = SendRequest(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Item> ListItemsForUser(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/items", Method.GET);
            request.AddUrlSegment("id", userId);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                return JsonConvert.DeserializeObject<List<Item>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<Item>();
        }

        public IEnumerable<PayPalAccount> ListPayPalAccountsForUser(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/paypal_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            IRestResponse response;
            try
            {
                response = SendRequest(Client, request);
            }
            catch (ApiErrorsException e)
            {
                if (e.Errors.Count == 1 && e.Errors.Values.First().First() == "no account found")
                {
                    return new List<PayPalAccount>();
                }
                throw;
            }
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("paypal_accounts"))
            {
                var itemCollection = dict["paypal_accounts"];
                return JsonConvert.DeserializeObject<List<PayPalAccount>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<PayPalAccount>();
        }

        public IEnumerable<CardAccount> ListCardAccountsForUser(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/card_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            IRestResponse response;
            try
            {
                response = SendRequest(Client, request);
            }
            catch (ApiErrorsException e)
            {
                if (e.Errors.Count == 1 && e.Errors.Values.First().First() == "no account found")
                {
                    return new List<CardAccount>();
                }
                throw;
            }
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("card_accounts"))
            {
                var itemCollection = dict["card_accounts"];
                return JsonConvert.DeserializeObject<List<CardAccount>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<CardAccount>();
        }

        public IEnumerable<BankAccount> ListBankAccountsForUser(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/bank_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            IRestResponse response;
            try
            {
                response = SendRequest(Client, request);
            }
            catch (ApiErrorsException e)
            {
                if (e.Errors.Count == 1 && e.Errors.Values.First().First() == "no account found")
                {
                    return new List<BankAccount>();
                }
                throw e;
            }
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("bank_accounts"))
            {
                var itemCollection = dict["bank_accounts"];
                return JsonConvert.DeserializeObject<List<BankAccount>>(JsonConvert.SerializeObject(itemCollection));
            }
            
            return new List<BankAccount>();
        }

        public DisbursementAccount SetDisbursementAccount(string userId, string accountId)
        {
            AssertIdNotNull(userId);

            var request = new RestRequest("/users/{id}/disbursement_account", Method.PATCH);
            request.AddUrlSegment("id", userId);
            request.AddParameter("account_id", accountId);
            IRestResponse response;
            try
            {
                response = SendRequest(Client, request);
            }
            catch (ApiErrorsException)
            {
                throw;
            }
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var itemCollection = dict["users"];
                return JsonConvert.DeserializeObject<DisbursementAccount>(JsonConvert.SerializeObject(itemCollection));
            }

            return null;
        }

        public User UpdateUser(User user)
        {
            ValidateUser(user);
            var request = new RestRequest("/users/{id}", Method.PATCH);
            request.AddUrlSegment("id", user.Id);
            request.AddParameter("id", user.Id);
            request.AddParameter("first_name", user.FirstName);
            request.AddParameter("last_name", user.LastName);
            request.AddParameter("email", user.Email);
            request.AddParameter("mobile", user.Mobile);
            request.AddParameter("address_line1", user.AddressLine1);
            request.AddParameter("address_line2", user.AddressLine2);
            request.AddParameter("state", user.State);
            request.AddParameter("city", user.City);
            request.AddParameter("zip", user.Zip);
            request.AddParameter("country", user.Country);

            request.AddParameter("drivers_license", user.DriversLicense);
            request.AddParameter("government_number", user.GovernmentNumber);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, User>>(response.Content).Values.First();
        }
        #endregion

        #region private methods

        private void ValidateUser(User user)
        {
            if (String.IsNullOrEmpty(user.Id))
            {
                throw new ValidationException("Field User.ID should not be empty!");
            }
            if (String.IsNullOrEmpty(user.FirstName))
            {
                throw new ValidationException("Field User.FirstName should not be empty!");
            }
            if (!IsCorrectCountryCode(user.Country))
            {
                throw new ValidationException("Field User.Country should contain 3-letter ISO country code!");
            }
            if (!IsCorrectEmail(user.Email))
            {
                throw new ValidationException("Field User.Email should contain correct email address!");
            }
        }

        #endregion
    }
}
