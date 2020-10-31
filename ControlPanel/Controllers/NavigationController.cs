using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public PartialViewResult Menu()
        {
            ViewBag.SelectedCategory=RouteData.Values["controller"];
            return PartialView();
        }
    }
}