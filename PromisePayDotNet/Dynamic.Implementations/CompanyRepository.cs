using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PromisePayDotNet.Dynamic.Implementations
{
    public class CompanyRepository : PromisePayDotNet.Implementations.AbstractRepository,
                                         PromisePayDotNet.Dynamic.Interfaces.ICompanyRepository
    {
        public CompanyRepository(IRestClient client, ILoggerFactory loggerFactory, IOptions<Settings.PromisePaySettings> options)
            : base(client, loggerFactory.CreateLogger<CompanyRepository>(), options)
        {
        }


        public IEnumerable<IDictionary<string,object>> ListCompanies()
        {
            var request = new RestRequest("/companies", Method.GET);

            var response = SendRequest(Client, request);
            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
            if (dict.ContainsKey("companies"))
            {
                var uploadCollection = dict["companies"];
                return JsonConvert.DeserializeObject<List<IDictionary<string, object>>>(JsonConvert.SerializeObject(uploadCollection));
            }
            return new List<IDictionary<string, object>>();
        }

        public IDictionary<string, object> GetCompanyById(string companyId)
        {
            AssertIdNotNull(companyId);
            var request = new RestRequest("/companies/{id}", Method.GET);
            request.AddUrlSegment("id", companyId);
            var response = SendRequest(Client, request);
            var company = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(company));
        }

        public IDictionary<string, object> CreateCompany(IDictionary<string, object> company)
        {
            var request = new RestRequest("/companies", Method.POST);

            foreach (var key in company.Keys) {
                request.AddParameter(key, (string)company[key]);
            }
            var response = SendRequest(Client, request);
            var returnedCompany = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(returnedCompany));
        }

        public IDictionary<string, object> EditCompany(IDictionary<string, object> company)
        {
            var request = new RestRequest("/companies", Method.POST);

            foreach (var key in company.Keys) {
                request.AddParameter(key, (string)company[key]);
            }

            var response = SendRequest(Client, request);
            var returnedCompany = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content).Values.First();
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(returnedCompany));
        }
    }
}
