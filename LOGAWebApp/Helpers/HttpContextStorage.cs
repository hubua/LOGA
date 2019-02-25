using System;
using LOGAWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace LOGAWebApp.Helpers
{
    public class HttpContextStorage
    {
        private const string USER_SETTINGS = "USER_SETTINGS";

        public static UserSettings GetUserSettings(HttpContext context)
        {
            return (UserSettings)context.Items[USER_SETTINGS] ?? new UserSettings { WritingCapitalization = Writing.Mixed, SaveLearnProgress = true, SavedLearnProgressLId = 0 };
        }

        public static void SetUserSettings(HttpContext context, UserSettings settings)
        {
            context.Items[USER_SETTINGS] = settings;
        }

        public static int GetUserLearnProgressLId(HttpContext context)
        {
            return GetUserSettings(context).SavedLearnProgressLId;
        }

        public static void SetUserLearnProgressLId(HttpContext context, int lid)
        {
            var settings = GetUserSettings(context);
            settings.SavedLearnProgressLId = lid;
            SetUserSettings(context, settings);
        }
    }
}