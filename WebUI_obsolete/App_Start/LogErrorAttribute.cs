using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI
{
    public class LogErrorAttribute : HandleErrorAttribute
    {

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var userIP = filterContext.HttpContext.Request.UserHostAddress;
            var date = DateTime.Now;
            var filename = $"error_{date.ToString("yyyy-MM-dd_HH-mm-ss")}.txt";
            using (var file = System.IO.File.CreateText(filterContext.HttpContext.Server.MapPath(@"~\_ErrorLogs\" + filename)))
            {
                file.WriteLine($"{date.ToLongDateString()} {date.ToLongTimeString()} - {userIP}");
                file.WriteLine(filterContext.Exception.GetInnerMessage());
                file.WriteLine(filterContext.Exception.StackTrace);
            }

        }
    }

    public static class ExceptionExtenstion
    {
        public static string GetInnerMessage(this Exception source)
        {
            if (source.InnerException == null)
            {
                return source.Message;
            }
            else
            {
                return String.Concat(source.Message, "->", source.InnerException.GetInnerMessage());
            }
        }
    }
}