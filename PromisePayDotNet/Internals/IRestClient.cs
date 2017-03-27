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
    public interface IAuthenticator { }
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
    public class RestRequest
    {
        internal string url;

        public RestRequest(string url, string method)
        {
            this.url = url;
            this.Method = method;
        }

        public string Method { get; }

        internal void AddUrlSegment(string name, string value)
        {
            this.url = this.url.Replace($"{{{name}}}", WebUtility.UrlEncode(value));
        }

        internal void AddParameter(string name, object value)
        {
            if (ReferenceEquals(null, value)) throw new NullReferenceException(nameof(value));
            if (this.url.Contains("?"))
            {
                this.url = $"{this.url}&{WebUtility.UrlEncode(name)}={WebUtility.UrlEncode(value.ToString())}";
            }
            else
            {
                this.url = $"{this.url}?{WebUtility.UrlEncode(name)}={WebUtility.UrlEncode(value.ToString())}";
            }
        }
    }
    public class Method
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string PATCH = "PATCH";
        public const string DELETE = "DELETE";
    }
    public class RestResponse
    {
        public virtual Uri ResponseUri { get; set; }
        public virtual string StatusDescription { get; set; }
        public virtual string Content { get; set; }
        public virtual HttpStatusCode StatusCode { get; set; }
    }
}
