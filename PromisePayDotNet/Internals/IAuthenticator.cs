using System;
using System.Net;
using System.Net.Http;

namespace PromisePayDotNet.Internals
{
    public interface IAuthenticator
    {
        void Add(HttpRequestMessage req);
    }
}
