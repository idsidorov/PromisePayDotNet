using Newtonsoft.Json;
using PromisePayDotNet.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using PromisePayDotNet.Settings;
using Microsoft.Extensions.Logging;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class FeeRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.IFeeRepository
    {
        public FeeRepository(IRestClient client, ISettings settings, ILogger<FeeRepository> logger) : base(client, settings, logger)
        {
        }

        #region public methods
        public IDictionary<string,object> ListFees()
        {
            var request = new RestRequest("/fees", Method.GET);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string,object> GetFeeById(string feeId)
        {
            AssertIdNotNull(feeId);
            var request = new RestRequest("/fees/{id}", Method.GET);
            request.AddUrlSegment("id", feeId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }

        public IDictionary<string,object> CreateFee(IDictionary<string, object> fee)
        {
            VailidateFee(fee);
            var request = new RestRequest("/fees", Method.POST);

            foreach (var key in fee.Keys) {
                request.AddParameter(key, (string)fee[key]);            
            }

            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
        }
        #endregion

        #region private methods
        private void VailidateFee(IDictionary<string, object> fee)
        {
            if (fee == null) throw new ArgumentNullException("fee");
            if (!_possibleTos.Contains((string)fee["to"]))
            {
                throw new ValidationException(
                    "To should have value of \"buyer\", \"seller\", \"cc\", \"int_wire\", \"paypal_payout\"");
            }
        }
        #endregion

        private readonly List<string> _possibleTos = new List<string> { "buyer", "seller", "cc", "int_wire", "paypal_payout" };
    }
}
