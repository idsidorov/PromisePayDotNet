using System;
using System.Net;
using System.Net.Http;

namespace PromisePayDotNet.Internals
{
    public class RestRequest
    {
        internal string url;
        public RestRequest(string url, HttpMethod method)
            : this(url, method, null)
        {
        }

        public RestRequest(string url, HttpMethod method, object body)
        {
            this.url = url;
            this.Method = method;
            this.Body = body;
        }
        /// <summary>
        /// Gets the method.
        /// </summary>
        public HttpMethod Method { get; }
        /// <summary>
        /// Gets the body of the request.
        /// </summary>
        public object Body { get; }

        internal void AddUrlSegment(string name, string value)
        {
            this.url = this.url.Replace($"{{{name}}}", Uri.EscapeDataString(value));
        }

        internal void AddParameter(string name, object value)
        {
            if (ReferenceEquals(null, value)) return;
            if (this.url.Contains("?"))
            {
                this.url = $"{this.url}&{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value.ToString())}";
            }
            else
            {
                this.url = $"{this.url}?{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value.ToString())}";
            }
        }
    }
}
