using Newtonsoft.Json;
using NUnit.Framework;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicCompanyTest : AbstractTest
    {
        [Test]
        public void CompanyDeserialization()
        {
            const string jsonStr = "{ \"legal_name\": \"Igor\", \"name\": null, \"id\": \"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\", \"related\": { \"address\": \"07ed45e5-bb9d-459f-bb7b-a02ecb38f443\" }, \"links\": { \"self\": \"/companies/e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\" } }";
            var company = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.IsNotNull(company);
            Assert.AreEqual("Igor", (string)company["legal_name"]);
            Assert.AreEqual("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5", (string)company["id"]);
        }

        [Test]
        public void ListCompaniesSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_list.json");

            var client = GetMockClient(content);
            var repo = new CompanyRepository(client.Object, GetMockSettings().Object, GetMockLogger<CompanyRepository>().Object);
            var resp = repo.ListCompanies();
            client.VerifyAll();
            Assert.IsNotNull(resp);
            var companies = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(JsonConvert.SerializeObject(resp["companies"]));
            Assert.IsTrue(companies.Count()>0);
        }

        [Test]
        public void GetCompanyByIdSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_get_by_id.json");

            var client = GetMockClient(content);
            var repo = new CompanyRepository(client.Object, GetMockSettings().Object, GetMockLogger<CompanyRepository>().Object);
            var resp = repo.GetCompanyById("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5");
            client.VerifyAll();
            var first = resp.Values.First();
            var company = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(first));

            Assert.IsNotNull(company);
            Assert.AreEqual("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5", (string)company["id"]);
        }

        [Test]
        public void CreateCompanySuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_create.json");

            var client = GetMockClient(content);
            var repo = new CompanyRepository(client.Object, GetMockSettings().Object, GetMockLogger<CompanyRepository>().Object);
            var resp = repo.CreateCompany(new Dictionary<string, object>
            { { "legal_name", "Test company #1" },
                { "name", "Test company #1" },
                {"country", "AUS"},
                {"tax_number", string.Empty},
                {"charge_tax", string.Empty},
                {"address_line1", string.Empty},
                {"address_line2", string.Empty},
                {"city", string.Empty},
                {"state", string.Empty},
                {"zip", string.Empty}
            });
            client.VerifyAll();
            var returnedCompany = resp.Values.First();
            var createdCompany = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(returnedCompany));

            Assert.IsNotNull(createdCompany);
            Assert.IsNotNull(createdCompany["id"]);
            Assert.AreEqual("Test company #1", (string)createdCompany["legal_name"]);
        }

        [Test]
        public void EditCompanySuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_edit.json");

            var client = GetMockClient(content);
            var repo = new CompanyRepository(client.Object, GetMockSettings().Object, GetMockLogger<CompanyRepository>().Object);
            var resp = repo.EditCompany(new Dictionary<string,object>
            {
                {"id" , "739dcfc5-adf0-4a00-b639-b4e05922994d"},
                {"legal_name" , "Test company #2"},
                {"name" , "Test company #2"},
                {"country" , "AUS"},
                {"tax_number", string.Empty},
                {"charge_tax", string.Empty},
                {"address_line1", string.Empty},
                {"address_line2", string.Empty},
                {"city", string.Empty},
                {"state", string.Empty},
                {"zip", string.Empty}
            });
            client.VerifyAll();
            var returnedCompany = resp.Values.First();
            var editedCompany = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(returnedCompany));

            Assert.AreEqual("Test company #2", (string)editedCompany["name"]);
        }


    }
}
