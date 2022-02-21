using Newtonsoft.Json;

namespace DataTable
{
    public class TableColumn
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("searchable")]
        public bool searchable { get; set; }
        [JsonProperty("orderable")]
        public bool orderable { get; set; }
        [JsonProperty("search")]
        public SearchValue search { get; set; }
    }
}
