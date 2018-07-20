using System;
using System.Collections.Generic;
using System.Linq;
using LOGA.WebUI.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LOGA.WebUI
{
    public class UserSettingsActionFilter : IActionFilter //TODO simplify
    {
        private const string COOKIE_USERSETTINGS = "COOKIE_USERSETTINGS";
        //private const string COOKIE_VALUE_DISPLAYNAME = "COOKIE_VALUE_DISPLAYNAME";
        //private const string COOKIE_VALUE_LEARN_ASOMTAVRULI = "COOKIE_VALUE_LEARN_ASOMTAVRULI";
        //private const string COOKIE_VALUE_LEARN_PROGRESS_LID = "COOKIE_VALUE_LEARN_PROGRESS_LID";

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
                var settings = new UserSettings();
                int progressLId = 0;

                settings = JsonConvert.DeserializeObject<UserSettings>(cookie);

                HttpContextStorage.SetUserSettings(filterContext.HttpContext, settings);
                HttpContextStorage.SetUserLearnProgressLId(filterContext.HttpContext, progressLId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var settings = HttpContextStorage.GetUserSettings(filterContext.HttpContext);
            int progressLId = HttpContextStorage.GetUserLearnProgressLId(filterContext.HttpContext);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents XSS cookie stealing with clientside JS
                Expires = DateTime.Now.AddYears(1), // TODO: set past date if no user name
            };

            string displaynameUFT = settings.DisplayName;
            string displaynameASCII = UTF2ASCII(displaynameUFT);

            filterContext.HttpContext.Response.Cookies.Append(COOKIE_USERSETTINGS, JsonConvert.SerializeObject(settings), cookieOptions);
        }
    }

    public class HttpContextStorage //TODO sep class
    {
        private const string USER_DISPLAYNAME = "USER_DISPLAYNAME";
        private const string USER_LEARN_ASOMTAVRULI = "USER_LEARN_ASOMTAVRULI";
        private const string USER_LEARN_PROGRESS_LID = "USER_LEARN_PROGRESS_LID";

        public static UserSettings GetUserSettings(HttpContext context)
        {
            return new UserSettings
            {
                DisplayName = context.Items[USER_DISPLAYNAME]?.ToString(),
                LearnAsomtavruli = context.Items[USER_LEARN_ASOMTAVRULI]?.ToString() == true.ToString(),
            };
        }

        public static void SetUserSettings(HttpContext context, UserSettings settings)
        {
            context.Items[USER_DISPLAYNAME] = settings.DisplayName;
            context.Items[USER_LEARN_ASOMTAVRULI] = settings.LearnAsomtavruli;
        }

        public static bool HasProgressSaved(HttpContext context)
        {
            return !String.IsNullOrEmpty(context.Items[USER_DISPLAYNAME]?.ToString());
        }

        public static int GetUserLearnProgressLId(HttpContext context)
        {
            int progressLId = 0;
            if (!String.IsNullOrEmpty(context.Items[USER_DISPLAYNAME]?.ToString()))
            {
                Int32.TryParse(context.Items[USER_LEARN_PROGRESS_LID]?.ToString(), out progressLId);
            }
            return progressLId;
        }

        public static void SetUserLearnProgressLId(HttpContext context, int lid)
        {
            context.Items[USER_LEARN_PROGRESS_LID] = lid;
        }
    }

    public static class RequestExtension //TODO sep class
    {
        //regex from http://detectmobilebrowsers.com/
        private static readonly Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public static bool IsMobileBrowser(this HttpRequest request)
        {
            var userAgent = request.UserAgent();
            if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))))
            {
                return true;
            }

            return false;
        }

        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"];
        }
    }

    public static class SessionExtensions // TODO sep class
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
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