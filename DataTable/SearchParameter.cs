using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataTable
{
    public abstract class SearchParameter<T>
    {
        [JsonProperty("draw")]
        public int draw { get; set; }
        [JsonProperty("columns")]
        public TableColumn[] columns { get; set; }
        [JsonProperty("order")]
        public SortOrder[] order { get; set; }
        [JsonProperty("start")]
        public int start { get; set; }
        [JsonProperty("length")]
        public int length { get; set; }
        [JsonProperty("search")]
        public SearchText search { get; set; }
        public string SortColumn => columns != null && order != null && order.Length > 0
                   ? (columns[order[0].Column].Data + " " + order[0].dir)
                   : null;

        public long RecordsFiltered { get; set; }
        public long RecordsTotal { get; set; }
        public List<T> Data { get; set; }
        [JsonProperty("filterBy")]
        public CustomFilterBy[] FilterBy { get; set; }
    }
   
}
