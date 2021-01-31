using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace ControlPanel.Filters
{
    public class ActionStartAttribute : FilterAttribute, IActionFilter
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //foreach(var qq in filterContext.ActionParameters)
            //{
            //    logger.Debug(qq.Key);
            //    logger.Error(qq.Value);
            //}
            //string logString = filterContext.RouteData.Values["controller"].ToString()
            //    + " || "
            //    + filterContext.RouteData.Values["action"].ToString()
            //    + " || ";
            //logger.Info($"QQQ {logString}");
        }
    }
}