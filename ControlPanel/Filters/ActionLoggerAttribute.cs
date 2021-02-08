using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace ControlPanel.Filters
{
    public class ActionLoggerAttribute : FilterAttribute, IActionFilter
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            logger.Info($"Action End | Controller name: {filterContext.RouteData.Values["controller"]} " +
                $"| Action name: {filterContext.RouteData.Values["action"]}" +
                $"| Method: {filterContext.HttpContext.Request.HttpMethod}");
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string logString= $"Action Start | Controller name: {filterContext.RouteData.Values["controller"]} " +
                $"| Action name: {filterContext.RouteData.Values["action"]}" +
                $"| Method: {filterContext.HttpContext.Request.HttpMethod}" +
                $"| Input params: ";

            foreach (var keyValuePair in filterContext.ActionParameters)
            {
                logString += $"{keyValuePair.Key}={keyValuePair.Value} | ";
            }

            logger.Info($"{logString}");
        }
    }
}