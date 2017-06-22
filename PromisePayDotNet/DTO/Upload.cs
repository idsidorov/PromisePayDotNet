using Newtonsoft.Json;

namespace PromisePayDotNet.Dto
{
    public class Upload : AbstractDTO
    {
        [JsonProperty(PropertyName = "total_lines")]
        public int? TotalLines { get; set; }

        [JsonProperty(PropertyName = "processed_lines")]
        public int? ProcessedLines { get; set; }

        [JsonProperty(PropertyName = "update_lines")]
        public int? UpdateLines { get; set; }

        [JsonProperty(PropertyName = "error_lines")]
        public int? ErrorLines { get; set; }

        [JsonProperty(PropertyName = "progress")]
        public double? progress { get; set; }

    }
}
