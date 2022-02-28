using System.Collections.Generic;
using System.Linq;
using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECom.Web.Common
{
    public static class SelectedListHelper
    {
        public static List<SelectListItem> GetProductCategorySelectList(List<ProductCategory> model, int selectedId, bool addPleaseSelect = true)
        {
            var roleSelectList = new List<SelectListItem>();
            roleSelectList.AddRange(model.Where(x => !string.IsNullOrWhiteSpace(x?.Name)).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Selected = selectedId == x.Id,
                Text = x.Name
            }));
            return roleSelectList.OrderBy(x => x.Text).ToList();
        }

    }
}