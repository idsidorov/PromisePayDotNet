using System;
using System.Net;

namespace PromisePayDotNet.Internals
{
    public class HttpBasicAuthenticator : IAuthenticator
    {
        private string login;
        private string password;

        public HttpBasicAuthenticator(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
