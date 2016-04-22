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
        private const string COOKIE_VALUE_LEARN_ASOMTAVRULI = "COOKIE_VALUE_LEARN_ASOMTAVRULI";
        private const string COOKIE_VALUE_LEARN_PROGRESS_LID = "COOKIE_VALUE_LEARN_PROGRESS_LID";

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[COOKIE_USERSETTINGS];

            if (cookie != null)
            {
                var settings = new UserSettings();
                int progressLId = 0;

                string displayname = String.Empty;
                try
                {
                    var b = Convert.FromBase64String(cookie.Values[COOKIE_VALUE_DISPLAYNAME]);
                    displayname = new String(System.Text.Encoding.UTF8.GetChars(b));

                    throw new Exception("a");
                }
                catch (Exception)
                {
                    // TODO: Log conversion exception                    
                }

                settings.DisplayName = displayname;
                settings.LearnAsomtavruli = Convert.ToBoolean(cookie.Values[COOKIE_VALUE_LEARN_ASOMTAVRULI]);
                Int32.TryParse(cookie.Values[COOKIE_VALUE_LEARN_PROGRESS_LID], out progressLId);

                HttpContextStorage.SetUserSettings(filterContext.HttpContext, settings);
                HttpContextStorage.SetUserLearnProgressLId(filterContext.HttpContext, progressLId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var settings = HttpContextStorage.GetUserSettings(filterContext.HttpContext);
            int progressLId = HttpContextStorage.GetUserLearnProgressLId(filterContext.HttpContext);

            var cookie = new HttpCookie(COOKIE_USERSETTINGS);
            cookie.HttpOnly = true; // TODO: why do need this
            cookie.Expires = DateTime.Now.AddYears(1); // TODO: set past date if no user name

            string asciiname = String.Empty;
            try
            {
                asciiname = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(settings.DisplayName));
            }
            catch (Exception)
            {
                // TODO: Log conversion error
            }

            cookie.Values.Add(COOKIE_VALUE_DISPLAYNAME, asciiname);
            cookie.Values.Add(COOKIE_VALUE_LEARN_ASOMTAVRULI, settings.LearnAsomtavruli.ToString());
            cookie.Values.Add(COOKIE_VALUE_LEARN_PROGRESS_LID, progressLId.ToString());

            filterContext.HttpContext.Response.SetCookie(cookie);
        }
    }

    public class HttpContextStorage
    {
        private const string USER_DISPLAYNAME = "USER_DISPLAYNAME";
        private const string USER_LEARN_ASOMTAVRULI = "USER_LEARN_ASOMTAVRULI";
        private const string USER_LEARN_PROGRESS_LID = "USER_LEARN_PROGRESS_LID";

        public static bool HasSettingsSaved(HttpContextBase context)
        {
            return !String.IsNullOrEmpty(context.Items[USER_DISPLAYNAME]?.ToString());
        }

        public static UserSettings GetUserSettings(HttpContextBase context)
        {
            return new UserSettings
            {
                DisplayName = context.Items[USER_DISPLAYNAME]?.ToString(),
                LearnAsomtavruli = context.Items[USER_LEARN_ASOMTAVRULI]?.ToString() == true.ToString(),
            };
        }

        public static void SetUserSettings(HttpContextBase context, UserSettings settings)
        {
            context.Items[USER_DISPLAYNAME] = settings.DisplayName;
            context.Items[USER_LEARN_ASOMTAVRULI] = settings.LearnAsomtavruli;
        }

        public static int GetUserLearnProgressLId(HttpContextBase context)
        {
            int progressLId = 0;
            if (!String.IsNullOrEmpty(context.Items[USER_DISPLAYNAME]?.ToString()))
            {
                Int32.TryParse(context.Items[USER_LEARN_PROGRESS_LID]?.ToString(), out progressLId);
            }
            return progressLId;
        }

        public static void SetUserLearnProgressLId(HttpContextBase context, int lid)
        {
            context.Items[USER_LEARN_PROGRESS_LID] = lid;
        }
    }

}