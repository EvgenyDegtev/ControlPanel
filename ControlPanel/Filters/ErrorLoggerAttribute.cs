using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Filters
{
    public class ErrorLoggerAttribute: FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void OnException(ExceptionContext filterContext)
        {
            logger.Error($"\n| Controller name: {filterContext.RouteData.Values["controller"].ToString()} \n" +
                $"| Action name: {filterContext.RouteData.Values["action"].ToString()} \n" +
                $"| Exception Message: {filterContext.Exception.Message} \n" +
                $"| StackTrace: {filterContext.Exception.StackTrace}"); 

            //filterContext.ExceptionHandled = true;
        }
    }
}