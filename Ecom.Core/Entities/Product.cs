using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ECom.Core.Entities
{
    public class Product
    {
        [Key]
        //[JsonPropertyName("id")]
        public int Id { get; set; }
        //[JsonPropertyName("name")]
        public string Name { get; set; }
        //[JsonPropertyName("sku")]
        public string SKU { get; set; }
        //[JsonPropertyName("slug")]
        public string Slug { get; set; }
        //[JsonPropertyName("isfeatured")]
        public bool IsFeatured { get; set; }// Featured products are displayed on home page
        //[JsonPropertyName("imageurl")]
        public string ImageUrl { get; set; }// Image URL of the main image
        //[JsonPropertyName("createddate")]
        public DateTime CreatedDate { get; set; }
        //[JsonPropertyName("description")]
        public string Description { get; set; }
        //[JsonPropertyName("price")]
        public decimal Price { get; set; }
        //[JsonPropertyName("rating")]
        public float? Rating { get; set; }
        //[JsonPropertyName("brand")]
        public string Brand { get; set; }
        //[JsonPropertyName("reviewcount")]
        public int ReviewCount { get; set; }
        //[JsonPropertyName("stockcount")]
        public int StockCount { get; set; }
        //[JsonPropertyName("productcategoryid")]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }// Multiple images for carosel view
    }
}
