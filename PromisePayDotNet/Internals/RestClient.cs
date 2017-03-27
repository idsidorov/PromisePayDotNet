using System;

namespace PromisePayDotNet.Internals
{

    internal class RestClient : IRestClient
    {
        public Uri BaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAuthenticator Authenticator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RestResponse Execute(RestRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
