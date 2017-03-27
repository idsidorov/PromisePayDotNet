using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PromisePayDotNet.Implementations
{
    internal class FeeRepository : AbstractRepository, IFeeRepository
    {
        public FeeRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<FeeRepository>(), options)
        {
        }

        public IEnumerable<Fee> ListFees()
        {
            var request = new RestRequest("/fees", Method.GET);
            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("fees"))
            {
                var userCollection = dict["fees"];
                return JsonConvert.DeserializeObject<List<Fee>>(JsonConvert.SerializeObject(userCollection));
            }
            return new List<Fee>();
        }

        public Fee GetFeeById(string feeId)
        {
            AssertIdNotNull(feeId);
            var request = new RestRequest("/fees/{id}", Method.GET);
            request.AddUrlSegment("id", feeId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Fee>>(response.Content).Values.First();
        }

        public Fee CreateFee(Fee fee)
        {
            VailidateFee(fee);
            var request = new RestRequest("/fees", Method.POST);
            request.AddParameter("name", fee.Name);
            request.AddParameter("fee_type_id", (int)fee.FeeType);
            request.AddParameter("amount", fee.Amount);
            request.AddParameter("cap", fee.Cap);
            request.AddParameter("min", fee.Min);
            request.AddParameter("max", fee.Max);
            request.AddParameter("to", fee.To);

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Fee>>(response.Content).Values.First();
        }

        private void VailidateFee(Fee fee)
        {
            if (fee == null) throw new ArgumentNullException("fee");
            if (!_possibleTos.Contains(fee.To))
            {
                throw new ValidationException(
                    "To should have value of \"buyer\", \"seller\", \"cc\", \"int_wire\", \"paypal_payout\"");
            }
        }

        private readonly List<string> _possibleTos = new List<string> {"buyer", "seller", "cc", "int_wire", "paypal_payout"};
    }
}
