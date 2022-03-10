using Newtonsoft.Json;

namespace DataTable
{
    public class FilterBy
    {
        [JsonProperty("entityName")]
        public string EntityName { get; set; }
        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }
        [JsonProperty("propertyValue")]
        public string PropertyValue { get; set; }
    }
}
