using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Enums;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Implementations;
using System;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class FeeTest : AbstractTest
    {
        [Test]
        public void FeeDeserialization()
        {
            const string jsonStr = "{ \"id\": \"58e15f18-500e-4cdc-90ca-65e1f1dce565\", \"created_at\": \"2014-12-29T08:31:42.168Z\", \"updated_at\": \"2014-12-29T08:31:42.168Z\", \"name\": \"Buyer Fee @ 10%\", \"fee_type_id\": 2, \"amount\": 1000, \"cap\": null, \"min\": null, \"max\": null, \"to\": \"buyer\", \"links\": { \"self\": \"/fees/58e15f18-500e-4cdc-90ca-65e1f1dce565\" } }";
            var fee = JsonConvert.DeserializeObject<Fee>(jsonStr);
            Assert.IsNotNull(fee);
            Assert.AreEqual("58e15f18-500e-4cdc-90ca-65e1f1dce565", fee.Id);
            Assert.AreEqual("Buyer Fee @ 10%", fee.Name);
        }

        [Test]
        public void CreateFeeSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/fees_create.json");
            var client = GetMockClient(content);

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
            var feeId = Guid.NewGuid().ToString();
            var createdFee = repo.CreateFee(new Fee
            {
                Id = feeId,
                Amount = 1000,
                Name = "Test fee #1",
                FeeType = FeeType.Fixed,
                Cap = "1",
                Max = "3",
                Min = "2",
                To = "buyer"
            });
            Assert.IsNotNull(createdFee);
        }

        [Test]
        public void CreateFeeWrongTo()
        {
            var client = GetMockClient("");

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
            var feeId = Guid.NewGuid().ToString();
            Assert.Throws<ValidationException>(() => repo.CreateFee(new Fee
            {
                Id = feeId,
                Amount = 1000,
                Name = "Test fee #1",
                FeeType = FeeType.Fixed,
                Cap = "1",
                Max = "3",
                Min = "2",
            }));
        }

        [Test]
        public void GetFeeByIdSuccessfull()
        {
            var content = File.ReadAllText("./Fixtures/fees_get_by_id.json");
            var client = GetMockClient(content);

            var repo = new FeeRepository(client.Object, GetMockSettings().Object, GetMockLogger<FeeRepository>().Object);
            const string id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
            var fee = repo.GetFeeById(id);
            Assert.AreEqual(id, fee.Id);
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
