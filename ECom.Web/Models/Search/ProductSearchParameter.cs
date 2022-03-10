using DataTable;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ECom.Web.Models
{
    public class ProductSearchParameter : SearchParameter<Product>
    {
        public IEnumerable<SelectListItem> ProductCategorySelectList { get; set; }
    }
}
