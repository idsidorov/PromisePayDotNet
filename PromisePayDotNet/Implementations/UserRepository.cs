using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace PromisePayDotNet.Implementations
{
    internal class UserRepository : AbstractRepository, IUserRepository
    {
        public UserRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<UserRepository>(), options)
        {
        }

        #region public methods

        public async Task<IEnumerable<User>> ListUsersAsync(int limit = 10, int offset = 0, string search = null)
        {
            AssertListParamsCorrect(limit, offset);
            var request = new RestRequest("/users", Method.GET);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);
            request.AddParameter("search", search);

            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("users"))
            {
                var userCollection = dict["users"];
                return JsonConvert.DeserializeObject<List<User>>(JsonConvert.SerializeObject(userCollection));
            }
            return new List<User>();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}", Method.GET);
            request.AddUrlSegment("id", userId);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, User>>(response.Content).Values.First();
        }

        public async Task<User> CreateUserAsync(User user)
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

            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string,User>>(response.Content).Values.First();
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}", Method.DELETE);
            request.AddUrlSegment("id", userId);
            var response = await SendRequestAsync(Client, request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Item>> ListItemsForUserAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/items", Method.GET);
            request.AddUrlSegment("id", userId);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("items"))
            {
                var itemCollection = dict["items"];
                return JsonConvert.DeserializeObject<List<Item>>(JsonConvert.SerializeObject(itemCollection));
            }
            return new List<Item>();
        }

        public async Task<IEnumerable<PayPalAccount>> ListPayPalAccountsForUserAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/paypal_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            RestResponse response;
            try
            {
                response = await SendRequestAsync(Client, request);
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
                try
                {
                    return JsonConvert.DeserializeObject<List<PayPalAccount>>(JsonConvert.SerializeObject(itemCollection));
                }
                catch (JsonSerializationException)
                {
                    return new[] { JsonConvert.DeserializeObject<PayPalAccount>(JsonConvert.SerializeObject(itemCollection)) };
                }
                
            }
            return new List<PayPalAccount>();
        }

        public async Task<IEnumerable<CardAccount>> ListCardAccountsForUserAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/card_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            RestResponse response;
            try
            {
                response = await SendRequestAsync(Client, request);
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
                try
                {
                    return JsonConvert.DeserializeObject<List<CardAccount>>(JsonConvert.SerializeObject(itemCollection));
                }
                catch (JsonSerializationException)
                {
                    return new[] { JsonConvert.DeserializeObject<CardAccount>(JsonConvert.SerializeObject(itemCollection)) };
                }
            
            }
            return new List<CardAccount>();
        }

        public async Task<IEnumerable<BankAccount>> ListBankAccountsForUserAsync(string userId)
        {
            AssertIdNotNull(userId);
            var request = new RestRequest("/users/{id}/bank_accounts", Method.GET);
            request.AddUrlSegment("id", userId);
            RestResponse response;
            try
            {
                response = await SendRequestAsync(Client, request);
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
                try
                {
                    return JsonConvert.DeserializeObject<List<BankAccount>>(JsonConvert.SerializeObject(itemCollection));
                }
                catch (JsonSerializationException)
                {
                    return new[] { JsonConvert.DeserializeObject<BankAccount>(JsonConvert.SerializeObject(itemCollection)) };
                }
            }
            
            return new List<BankAccount>();
        }

        public async Task<DisbursementAccount> SetDisbursementAccountAsync(string userId, string accountId)
        {
            AssertIdNotNull(userId);

            var request = new RestRequest("/users/{id}/disbursement_account?account_id={account_id}", Method.POST);
            request.AddUrlSegment("id", userId);
            request.AddUrlSegment("account_id", accountId);
            RestResponse response;
            try
            {
                response = await SendRequestAsync(Client, request);
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

        public async Task<User> UpdateUserAsync(User user)
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

            var response = await SendRequestAsync(Client, request);
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
            if (!Email.IsCorrect(user.Email))
            {
                throw new ValidationException("Field User.Email should contain correct email address!");
            }
        }

        #endregion
    }
}
