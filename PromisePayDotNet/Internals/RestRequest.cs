using System;
using System.Net;

namespace PromisePayDotNet.Internals
{
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
}
