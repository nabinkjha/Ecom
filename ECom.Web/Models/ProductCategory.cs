using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECom.Web.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products{get; set;}
        [JsonIgnore]
        public string ErrorMessage { get; set; }
        [JsonIgnore]
        public string SuccessMessage { get; set; }
    }
}