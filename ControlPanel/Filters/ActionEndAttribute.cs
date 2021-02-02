using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace ControlPanel.Filters
{
    public class ActionEndAttribute : FilterAttribute, IActionFilter
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            logger.Info($"zzz Action End | Controller name: {filterContext.RouteData.Values["controller"].ToString()} | Action name: {filterContext.RouteData.Values["action"].ToString()}");

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