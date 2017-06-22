using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using PromisePayDotNet.Enums;

namespace PromisePayDotNet.Implementations
{
    internal class FeeRepository : AbstractRepository, IFeeRepository
    {
        public FeeRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<FeeRepository>(), options)
        {
        }

        public async Task<IEnumerable<Fee>> ListFeesAsync()
        {
            var request = new RestRequest("/fees", Method.GET);
            var response = await SendRequestAsync(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("fees"))
            {
                var userCollection = dict["fees"];
                return JsonConvert.DeserializeObject<List<Fee>>(JsonConvert.SerializeObject(userCollection));
            }
            return new List<Fee>();
        }

        public async Task<Fee> GetFeeByIdAsync(string feeId)
        {
            AssertIdNotNull(feeId);
            var request = new RestRequest("/fees/{id}", Method.GET);
            request.AddUrlSegment("id", feeId);
            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Fee>>(response.Content).Values.First();
        }

        class CreateFeeRequest
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "fee_type_id")]
            public int FeeType { get; set; }

            [JsonProperty(PropertyName = "amount")]
            public int Amount { get; set; }

            [JsonProperty(PropertyName = "cap")]
            public string Cap { get; set; }

            [JsonProperty(PropertyName = "min")]
            public string Min { get; set; }

            [JsonProperty(PropertyName = "max")]
            public string Max { get; set; }

            [JsonProperty(PropertyName = "to")]
            public FeeToType To { get; set; }
        }

        public async Task<Fee> CreateFeeAsync(Fee fee)
        {
            VailidateFee(fee);
            var request = new RestRequest("/fees", Method.POST, new CreateFeeRequest {
                Name = fee.Name,
                Amount = fee.Amount,
                Cap = fee.Cap,
                FeeType = (int)fee.FeeType,
                Max =fee.Max,
                Min = fee.Min,
                To = fee.To
            });

            var response = await SendRequestAsync(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Fee>>(response.Content).Values.First();
        }

        private void VailidateFee(Fee fee)
        {
            if (fee == null) throw new ArgumentNullException(nameof(fee));
            if (!_possibleTos.Contains(fee.To))
            {
                throw new ValidationException(
                    "To should have value of "+string.Join(", ", _possibleTos.Select(to=> $"\"{FeeToJsonConverter.ToString(to)}\"")));
            }
        }

        private readonly List<FeeToType> _possibleTos = new List<FeeToType> { 
               FeeToType.Buyer, FeeToType.Seller, FeeToType.CC, FeeToType.IntWire, FeeToType.PaypalPayout 
        };
    }
}
