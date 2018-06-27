using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI
{
    public class LogActionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //var f = System.IO.File.AppendText(filterContext.HttpContext.Server.MapPath();
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}