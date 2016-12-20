using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI.HtmlHelpers
{
    public static class HtmlHelperExtension
    {
        public static bool IsDebugMode(this HtmlHelper html)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}