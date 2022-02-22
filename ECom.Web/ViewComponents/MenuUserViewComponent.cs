using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ECom.Web.ViewComponents
{
    public class MenuUserViewComponent : ViewComponent
    {

        public MenuUserViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
