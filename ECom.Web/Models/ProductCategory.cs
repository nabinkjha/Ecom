using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;
namespace ECom.Web.Models
{
    public class ProductCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
        public ICollection<Product> Products{get; set;}
        [JsonIgnore]
        public string ErrorMessage { get; set; }
        [JsonIgnore]
        public string SuccessMessage { get; set; }
    }
}