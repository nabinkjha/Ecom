using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECom.Web.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }
        [JsonPropertyName("sku")]
        [Required]
        public string SKU { get; set; }
        [Required]
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
        [Required]
        [Range(1,100)]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("rating")]
        public float? Rating { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("reviewCount")]
        public int ReviewCount { get; set; }
        [JsonPropertyName("stockCount")]
        [Range(1, 100)]
        [Required]
        public int StockCount { get; set; }
        [Required]
        [JsonPropertyName("productCategoryId")]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
       
    }
}
