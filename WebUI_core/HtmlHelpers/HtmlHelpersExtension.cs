﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGA.WebUI.HtmlHelpers
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
