using System;
using System.Net;

namespace PromisePayDotNet.Internals
{
    public class RestResponse
    {
        public virtual Uri ResponseUri { get; set; }
        public virtual string StatusDescription { get; set; }
        public virtual string Content { get; set; }
        public virtual HttpStatusCode StatusCode { get; set; }
    }
}
