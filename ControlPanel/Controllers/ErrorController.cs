using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult StatusCode404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}