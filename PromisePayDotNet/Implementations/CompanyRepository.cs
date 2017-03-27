using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.DTO;
using PromisePayDotNet.Exceptions;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;


namespace PromisePayDotNet.Implementations
{
    internal class CompanyRepository : AbstractRepository, ICompanyRepository
    {
        public CompanyRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<CompanyRepository>(), options)
        {
        }

        public IEnumerable<Company> ListCompanies()
        {
            var request = new RestRequest("/companies", Method.GET);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("companies"))
            {
                var uploadCollection = dict["companies"];
                return JsonConvert.DeserializeObject<List<Company>>(JsonConvert.SerializeObject(uploadCollection));
            }
            return new List<Company>();
        }

        public Company GetCompanyById(string companyId)
        {
            AssertIdNotNull(companyId);
            var request = new RestRequest("/companies/{id}", Method.GET);
            request.AddUrlSegment("id", companyId);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Company>>(response.Content).Values.First();
        }

        public Company CreateCompany(Company company, string userId)
        {
            AssertIdNotNull(userId);
            if (!IsCorrectCountryCode(company.Country)) {
                throw new ValidationException("Field country should contain 3-letter ISO country code!");
            }
            var request = new RestRequest("/companies", Method.POST);
            request.AddParameter("name", company.Name);
            request.AddParameter("legal_name", company.LegalName);
            request.AddParameter("tax_number", company.TaxNumber);
            request.AddParameter("charge_tax", company.ChargeTax);
            request.AddParameter("address_line1", company.AddressLine1);
            request.AddParameter("address_line2", company.AddressLine2);
            request.AddParameter("city", company.City);
            request.AddParameter("state", company.State);
            request.AddParameter("zip", company.Zip);
            request.AddParameter("country", company.Country);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Company>>(response.Content).Values.First();
        }

        public Company EditCompany(Company company)
        {
            AssertIdNotNull(company.Id);
            var request = new RestRequest("/companies/{id}", Method.PATCH);
            request.AddUrlSegment("id", company.Id);
            request.AddParameter("name", company.Name);
            request.AddParameter("legal_name", company.LegalName);
            request.AddParameter("tax_number", company.TaxNumber);
            request.AddParameter("charge_tax", company.ChargeTax);
            request.AddParameter("address_line1", company.AddressLine1);
            request.AddParameter("address_line2", company.AddressLine2);
            request.AddParameter("city", company.City);
            request.AddParameter("state", company.State);
            request.AddParameter("zip", company.Zip);
            request.AddParameter("country", company.Country);
            request.AddParameter("phone", company.Phone);
            var response = SendRequest(Client, request);
            return JsonConvert.DeserializeObject<IDictionary<string, Company>>(response.Content).Values.First();
        }

    }
}
