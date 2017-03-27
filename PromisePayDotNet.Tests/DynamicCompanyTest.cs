using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Dynamic.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromisePayDotNet.Tests
{
    public class DynamicCompanyTest : AbstractTest
    {
        [Fact]
        public void CompanyDeserialization()
        {
            const string jsonStr = "{ \"legal_name\": \"Igor\", \"name\": null, \"id\": \"e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\", \"related\": { \"address\": \"07ed45e5-bb9d-459f-bb7b-a02ecb38f443\" }, \"links\": { \"self\": \"/companies/e466dfb4-f05c-4c7f-92a3-09a0a28c7af5\" } }";
            var company = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonStr);
            Assert.NotNull(company);
            Assert.Equal("Igor", (string)company["legal_name"]);
            Assert.Equal("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5", (string)company["id"]);
        }

        [Fact]
        public void ListCompaniesSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_list.json");

            var client = GetMockClient(content);
            var repo = Get<CompanyRepository>(client.Object);
            var companies = repo.ListCompanies();
            client.VerifyAll();
            Assert.NotNull(companies);
            Assert.True(companies.Any());
        }

        [Fact]
        public void GetCompanyByIdSuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_get_by_id.json");

            var client = GetMockClient(content);
            var repo = Get<CompanyRepository>(client.Object);
            var company = repo.GetCompanyById("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5");
            client.VerifyAll();
            Assert.NotNull(company);
            Assert.Equal("e466dfb4-f05c-4c7f-92a3-09a0a28c7af5", (string)company["id"]);
        }

        [Fact]
        public void CreateCompanySuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_create.json");

            var client = GetMockClient(content);
            var repo = Get<CompanyRepository>(client.Object);
            var createdCompany = repo.CreateCompany(new Dictionary<string, object>
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
            Assert.NotNull(createdCompany);
            Assert.NotNull(createdCompany["id"]);
            Assert.Equal("Test company #1", (string)createdCompany["legal_name"]);
        }

        [Fact]
        public void EditCompanySuccessfully()
        {
            var content = File.ReadAllText("./Fixtures/companies_edit.json");

            var client = GetMockClient(content);
            var repo = Get<CompanyRepository>(client.Object);
            var editedCompany = repo.EditCompany(new Dictionary<string,object>
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
            Assert.Equal("Test company #2", (string)editedCompany["name"]);
        }


    }
}
