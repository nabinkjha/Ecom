using Newtonsoft.Json;
namespace DataTable
{
    public class SearchValue
    {
        [JsonProperty("value")]
        public string Text { get; set; }
        [JsonProperty("regex")]
        public bool IsRegex { get; set; }
    }
}
