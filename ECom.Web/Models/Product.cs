using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ECom.Web.Models
{
    public class Product
    {
        public Product()
        {
           // ProductCategorySelectList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Slug { get; set; }
        public bool IsFeatured { get; set; }// Featured products are displayed on home page
        public string ImageUrl { get; set; }// Image URL of the main image
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float? Rating { get; set; }
        public string Brand { get; set; }
        public int ReviewCount { get; set; }
        public int StockCount { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        //public List<SelectListItem> ProductCategorySelectList { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
