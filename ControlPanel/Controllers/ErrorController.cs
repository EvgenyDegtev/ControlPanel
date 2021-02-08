using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Filters;

namespace ControlPanel.Controllers
{
    [ErrorLogger]
    [ActionLogger]
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