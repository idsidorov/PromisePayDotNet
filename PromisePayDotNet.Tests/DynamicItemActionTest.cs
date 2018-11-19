using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using PromisePayDotNet.Exceptions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PromisePayDotNet.Tests
{
    public class DynamicItemActionTest : AbstractTest
    {

        [Test]
        public void RequestPayment() 
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_payment.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RequestPayment("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #001", items["name"]);
        }

        [Test]
        public void MakePayment()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_make_payment.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.MakePayment("100fd4a0-0538-11e6-b512-3e1d05defe78", "5830def0-ffe8-11e5-86aa-5e5517507c66");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #001", items["name"]);
        }

        [Test]
        public void RequestRelease()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_release.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RequestRelease("100fd4a0-0538-11e6-b512-3e1d05defe78", 1000);
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #002", items["name"]);
        }

        [Test]
        public void ReleasePayment()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_release_payment.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.ReleasePayment("100fd4a0-0538-11e6-b512-3e1d05defe78", 1000);
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #002", items["name"]);
        }

        [Test]
        public void RequestRefund()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_refund.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RequestRefund("100fd4a0-0538-11e6-b512-3e1d05defe78", "1000", "Refund message");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }
        
        [Test]
        public void Refund()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_refund.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.Refund("100fd4a0-0538-11e6-b512-3e1d05defe78", "1000", "Refund message");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);

        }

        [Test]
        public void DeclineRefund()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_refund.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.DeclineRefund("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }

        [Test]
        public void RaiseDispute()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_raise_dispute.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RaiseDispute("100fd4a0-0538-11e6-b512-3e1d05defe78", "5830def0-ffe8-11e5-86aa-5e5517507c66");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }

        [Test]
        public void RequestDisputeResolution()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_dispute_resolution.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RequestDisputeResolution("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }

        [Test]
        public void ResolveDispute()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_resolve_dispute.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.ResolveDispute("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }

        [Test]
        public void EscalateDispute()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_escalate_dispute.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.EscalateDispute("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #003", items["name"]);
        }

        [Test]
        public void AcknowledgeWireTransfer()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_acknowledge_wire_transfer.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.AcknowledgeWire("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #001", items["name"]);
        }

        [Test]
        public void RevertWireTransfer()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_revert_wire_transfer.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RevertWire("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #001", items["name"]);
        }

        [Test]
        public void Cancel()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_cancel.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.Cancel("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #001", items["name"]);
        }

        [Test]
        public void SendTaxInvoice()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_send_tax_invoice.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.SendTaxInvoice("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #002", items["name"]);
        }

        [Test]
        public void RequestTaxInvoice()
        {
            var content = File.ReadAllText("./Fixtures/item_actions_request_tax_invoice.json");
            var client = GetMockClient(content);
            var repo = new ItemRepository(client.Object, GetMockSettings().Object, GetMockLogger<ItemRepository>().Object);

            var response = repo.RequestTaxInvoice("100fd4a0-0538-11e6-b512-3e1d05defe78");
            client.VerifyAll();
            var items = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(response["items"]));

            Assert.NotNull(items);
            Assert.AreEqual("Landscaping Job #002", items["name"]);
        }
    }
}
