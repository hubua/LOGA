using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOGA.WebUI.Models;

namespace LOGA.WebUI
{
    public class UserSettingsAttribute : FilterAttribute, IActionFilter
    {
        private const string COOKIE_USERSETTINGS = "COOKIE_USERSETTINGS";
        private const string COOKIE_VALUE_DISPLAYNAME = "COOKIE_VALUE_DISPLAYNAME";
        private const string COOKIE_VALUE_LEARNASOMTAVRULI = "COOKIE_VALUE_LEARNASOMTAVRULI";

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[COOKIE_USERSETTINGS];

            if (cookie != null)
            {
                var settings = new UserSettings();

                settings.DisplayName = cookie.Values[COOKIE_VALUE_DISPLAYNAME];
                settings.LearnAsomtavruli = Convert.ToBoolean(cookie.Values[COOKIE_VALUE_LEARNASOMTAVRULI]);

                HttpContextStorage.SetUserSettings(filterContext.HttpContext, settings);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var settings = HttpContextStorage.GetUserSettings(filterContext.HttpContext);

            var cookie = new HttpCookie(COOKIE_USERSETTINGS);
            cookie.Expires = DateTime.Now.AddYears(1); //httponly

            cookie.Values.Add(COOKIE_VALUE_DISPLAYNAME, settings.DisplayName);
            cookie.Values.Add(COOKIE_VALUE_LEARNASOMTAVRULI, settings.LearnAsomtavruli.ToString());

            filterContext.HttpContext.Response.SetCookie(cookie);
        }
    }

    public class HttpContextStorage
    {
        private const string USER_DISPLAYNAME = "USER_DISPLAYNAME";
        private const string USER_LEARN_ASOMTAVRULI = "USER_LEARN_ASOMTAVRULI";

        public static UserSettings GetUserSettings(HttpContextBase context)
        {
            return new UserSettings {
                DisplayName = context.Items[USER_DISPLAYNAME]?.ToString(),
                LearnAsomtavruli = context.Items[USER_LEARN_ASOMTAVRULI]?.ToString() == true.ToString()
            };
        }

        public static void SetUserSettings(HttpContextBase context, UserSettings settings)
        {
            context.Items[USER_DISPLAYNAME] = settings.DisplayName;
            context.Items[USER_LEARN_ASOMTAVRULI] = settings.LearnAsomtavruli;
        }
    }

}