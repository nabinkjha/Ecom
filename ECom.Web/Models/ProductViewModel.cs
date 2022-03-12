using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ECom.Web.Models
{
    public class ProductViewModel: Product
    {
        public string Category { get { return ProductCategory?.Name; } }
        public IEnumerable<SelectListItem> ProductCategorySelectList { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
    }
}
