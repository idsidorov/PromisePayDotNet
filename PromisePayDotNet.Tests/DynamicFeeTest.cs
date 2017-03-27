using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Dynamic.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicFeeTest : AbstractTest
    {
        [Fact]
        public void FeeDeserialization()
        {
            const string jsonStr = "{ \"id\": \"58e15f18-500e-4cdc-90ca-65e1f1dce565\", \"created_at\": \"2014-12-29T08:31:42.168Z\", \"updated_at\": \"2014-12-29T08:31:42.168Z\", \"name\": \"Buyer Fee @ 10%\", \"fee_type_id\": 2, \"amount\": 1000, \"cap\": null, \"min\": null, \"max\": null, \"to\": \"buyer\", \"links\": { \"self\": \"/fees/58e15f18-500e-4cdc-90ca-65e1f1dce565\" } }";
            var fee = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.NotNull(fee);
            Assert.Equal("58e15f18-500e-4cdc-90ca-65e1f1dce565", (string)fee["id"]);
            Assert.Equal("Buyer Fee @ 10%", (string)fee["name"]);
        }

        [Fact]
        public void CreateFeeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/fees_create.json");
            var client = GetMockClient(content);

            var repo = Get<FeeRepository>(client.Object);
            var feeId = Guid.NewGuid().ToString();
            var createdFee = repo.CreateFee(new Dictionary<string, object>
            { {   "id", feeId },
              {   "amount" , "1000"},
              {   "name" , "Test fee #1"},
              {   "fee_type_id" , "1"},
              {   "cap" , "1"},
              {   "max" , "3"},
              {   "min" , "2"},
              {   "to" , "buyer"}
            });
            Assert.NotNull(createdFee);
        }

        [Fact]
        public void CreateFeeWrongTo()
        {
            var client = GetMockClient("");

            var repo = Get<FeeRepository>(client.Object);
            var feeId = Guid.NewGuid().ToString();
            Assert.Throws<ValidationException>(() => repo.CreateFee(new Dictionary<string,object>
            { {   "id", feeId },
              {   "amount" , "1000"},
              {   "name" , "Test fee #1"},
              {   "fee_type_id" , "1"},
              {   "cap" , "1"},
              {   "max" , "3"},
              {   "min" , "2"},
              {   "to" , ""}
            }));
        }

        [Fact]
        public void GetFeeByIdSuccessfull()
        {
            var content = File.ReadAllText("./Fixtures/fees_get_by_id.json");
            var client = GetMockClient(content);

            var repo = Get<FeeRepository>(client.Object);
            const string id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
            var fee = repo.GetFeeById(id);
            Assert.Equal(id, (string)fee["id"]);
        }

        [Fact]
        public void ListFeeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/fees_list.json");
            var client = GetMockClient(content);

            var repo = Get<FeeRepository>(client.Object);
            var fees = repo.ListFees();
            Assert.NotNull(fees);
            Assert.True(fees.Any());
        }
    }
}
