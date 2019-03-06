using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeboenaWebApp.Extensions
{
    public static class HtmlHelperExtension
    {
        public static bool IsDebugMode(this IHtmlHelper html)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
