using System;
using System.Threading.Tasks;

namespace PromisePayDotNet.Internals
{
    public interface IRestClient
    {
        Uri BaseUrl { get; set; }
        IAuthenticator Authenticator { get; set; }

        Task<RestResponse> ExecuteAsync(RestRequest request);
    }
}
