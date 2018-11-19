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
    public class DynamicFeeTest : AbstractTest
    {
        [Test]
        public void FeeDeserialization()
        {
            const string jsonStr = "{ \"id\": \"58e15f18-500e-4cdc-90ca-65e1f1dce565\", \"created_at\": \"2014-12-29T08:31:42.168Z\", \"updated_at\": \"2014-12-29T08:31:42.168Z\", \"name\": \"Buyer Fee @ 10%\", \"fee_type_id\": 2, \"amount\": 1000, \"cap\": null, \"min\": null, \"max\": null, \"to\": \"buyer\", \"links\": { \"self\": \"/fees/58e15f18-500e-4cdc-90ca-65e1f1dce565\" } }";
            var fee = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.IsNotNull(fee);
            Assert.AreEqual("58e15f18-500e-4cdc-90ca-65e1f1dce565", (string)fee["id"]);
            Assert.AreEqual("Buyer Fee @ 10%", (string)fee["name"]);
        }

        [Test]
        public void CreateFeeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/fees_create.json");
            var client = GetMockClient(content);

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
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
            Assert.IsNotNull(createdFee);
        }

        [Test]
        public void CreateFeeWrongTo()
        {
            var client = GetMockClient("");

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
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

        [Test]
        public void GetFeeByIdSuccessfull()
        {
            var content = File.ReadAllText("./Fixtures/fees_get_by_id.json");
            var client = GetMockClient(content);

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
            const string id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
            var response = repo.GetFeeById(id);
            var fee = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response.Values.First()));

            Assert.AreEqual(id, (string)fee["id"]);
        }

        [Test]
        public void ListFeeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/fees_list.json");
            var client = GetMockClient(content);

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
            var fees = repo.ListFees();
            Assert.IsNotNull(fees);
            Assert.IsTrue(fees.Any());
        }
    }
}
