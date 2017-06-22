using PromisePayDotNet.Dto;
using System.Collections.Generic;

namespace PromisePayDotNet.Abstractions
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> ListCompanies();

        Company GetCompanyById(string companyId);

        Company CreateCompany(Company company, string userId);

        Company EditCompany(Company company);
    }
}
