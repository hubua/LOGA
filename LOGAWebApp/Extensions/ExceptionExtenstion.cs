using System;

namespace LOGAWebApp.Extensions
{
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