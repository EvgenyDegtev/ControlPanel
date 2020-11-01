using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ControlPanel.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public PartialViewResult Menu()
        {
            ViewBag.activeController = HttpContext.Request.RequestContext.RouteData.Values["Controller"];
            return PartialView();
        }
    }
}