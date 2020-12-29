using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ControlPanel.ViewModels;

namespace ControlPanel.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public PartialViewResult Menu()
        {
            NavViewModel navViewModel = new NavViewModel {ActiveController= HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString() };
            return PartialView(navViewModel);
        }
    }
}