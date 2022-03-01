using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECom.Web.Models
{
    public class Product
    {
        public Product()
        {
           // ProductCategorySelectList = new List<SelectListItem>();
        }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("sku")]
        public string SKU { get; set; }
        [JsonPropertyName("slug")]
        public string Slug { get; set; }
        [JsonPropertyName("isFeatured")]
        public bool IsFeatured { get; set; }// Featured products are displayed on home page
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }// Image URL of the main image
        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("rating")]
        public float? Rating { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("reviewCount")]
        public int ReviewCount { get; set; }
        [JsonPropertyName("stockCount")]
        public int StockCount { get; set; }
        [JsonPropertyName("productCategoryId")]
        public int ProductCategoryId { get; set; }
        
        public ProductCategory ProductCategory { get; set; }
        [JsonIgnore]
        public IEnumerable<SelectListItem> ProductCategorySelectList { get; set; }
        [JsonIgnore]
        public string ErrorMessage { get; set; }
        [JsonIgnore]
        public string SuccessMessage { get; set; }
    }
}
