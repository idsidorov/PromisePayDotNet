using System;
using System.Net;

namespace PromisePayDotNet.Internals
{
    public interface IRestClient
    {
        Uri BaseUrl { get; set; }
        IAuthenticator Authenticator { get; set; }

        RestResponse Execute(RestRequest request);
    }
}
