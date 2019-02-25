using System;
using System.Collections.Generic;
using System.Linq;
using LOGAWebApp.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using LOGAWebApp.Helpers;

namespace LOGAWebApp.Filters
{
    public class UserSettingsActionFilter : IActionFilter
    {
        private const string COOKIE_USERSETTINGS = "COOKIE_USERSETTINGS";

        private static string UTF2ASCII(string text)
        {
            string result = String.Empty;
            try
            {
                result = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
            }
            catch
            {
                // String corrupted
            }
            return result;
        }

        private static string ASCII2UTF(string text)
        {
            string result = String.Empty;
            try
            {
                var b = Convert.FromBase64String(text);
                result = new String(System.Text.Encoding.UTF8.GetChars(b));
            }
            catch
            {
                // String corrupted
            }
            return result;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[COOKIE_USERSETTINGS];

            if (cookie != null)
            {
                try
                {
                    var settings = JsonConvert.DeserializeObject<UserSettings>(cookie);
                    HttpContextStorage.SetUserSettings(filterContext.HttpContext, settings);
                }
                catch
                {
                    System.Console.WriteLine("WARNING: Unable to deserialize settings!");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var settings = HttpContextStorage.GetUserSettings(filterContext.HttpContext);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents XSS cookie stealing with clientside JS
                Expires = DateTime.Now.AddYears(1), // TODO: set past date if no user name
            };

            filterContext.HttpContext.Response.Cookies.Append(COOKIE_USERSETTINGS, JsonConvert.SerializeObject(settings), cookieOptions);
        }
    }

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute // TODO
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters#exception-filters
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilterAttribute(IHostingEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                // do nothing
                return;
            }

            var result = new ViewResult { ViewName = "CustomError" };
            result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            result.ViewData.Add("Exception", context.Exception);
            //TODO Pass additional detailed data via ViewData
            context.Result = result;
        }
    }

    public class LogErrorAttribute : ExceptionFilterAttribute // TODO
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var userIP = filterContext.HttpContext.Request.Host.Host;
            var date = DateTime.Now;
            var filename = $"error_{date.ToString("yyyy-MM-dd_HH-mm-ss")}.txt";
            /*using (var file = System.IO.File.CreateText(filterContext.HttpContext.Server.MapPath(@"~\_ErrorLogs\" + filename)))
            {
                file.WriteLine($"{date.ToLongDateString()} {date.ToLongTimeString()} - {userIP}");
                file.WriteLine(filterContext.Exception.GetInnerMessage());
                file.WriteLine(filterContext.Exception.StackTrace);
            }*/

        }
    }
}