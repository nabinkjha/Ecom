using Newtonsoft.Json;

namespace DataTable
{
    public class SearchText
    {
        [JsonProperty("value")]
        public string value { get; set; }
        [JsonProperty("regex")]
        public bool regex { get; set; }
    }
}
