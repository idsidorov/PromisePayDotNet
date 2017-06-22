using System.Collections.Generic;
using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class ErrorsDAO
    {
        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
