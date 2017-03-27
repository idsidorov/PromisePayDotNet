using System;
using System.Net;
using System.Net.Http;

namespace PromisePayDotNet.Internals
{
    internal class Method
    {
        public static HttpMethod GET = HttpMethod.Get;
        public static HttpMethod POST = HttpMethod.Post;
        public static HttpMethod PUT = HttpMethod.Put;
        public static HttpMethod PATCH = new HttpMethod("PATCH");
        public static HttpMethod DELETE = HttpMethod.Delete;
    }
}
