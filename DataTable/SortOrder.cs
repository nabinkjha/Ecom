using Newtonsoft.Json;
namespace DataTable
{
    public class SortOrder
    {
        [JsonProperty("column")]
        public int Column { get; set; }
        [JsonProperty("dir")]
        public string dir { get; set; }
    }
}
