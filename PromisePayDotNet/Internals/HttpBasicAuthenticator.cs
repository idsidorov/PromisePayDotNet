using System;
using System.Net.Http;
using System.Text;

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

        public void Add(HttpRequestMessage req)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}
