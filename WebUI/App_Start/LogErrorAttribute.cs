using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI
{
    public class LogErrorAttribute : HandleErrorAttribute
    {
        private const string SQL_QUERY =
            @"INSERT INTO ErrorLog
            (
                ExceptionType, InnerMessage, StackTrace, IsHandled, Controller, Action, [User], ErrorDate
            )
            VALUES
            (
                @ExceptionType, @InnerMessage,  @StackTrace, @IsHandled, @Controller, @Action, @User, @ErrorDate
            )";

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            // TODO: Log to storage
            //var a = filterContext.RequestContext.RouteData;

            /*
            using (SqlConnection conn = new SqlConnection(mssConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, conn))
                {
                    cmd.Parameters.AddWithValue("@ExceptionType", ExceptionType);
                    cmd.Parameters.AddWithValue("@InnerMessage", InnerMessage);
                    cmd.Parameters.AddWithValue("@StackTrace", StackTrace);
                    cmd.Parameters.AddWithValue("@IsHandled", false);
                    cmd.Parameters.AddWithValue("@Controller", Controller);
                    cmd.Parameters.AddWithValue("@Action", Action);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@ErrorDate", ErrorDate);

                    cmd.ExecuteNonQuery();
                }
            }
            */
        }
    }

    public static class ExceptionExtenstion
    {
        public static string GetInnerMessage(this Exception source, string separator = " -> ")
        {
            if (source.InnerException == null)
            {
                return source.Message;
            }
            else
            {
                return String.Concat(source.Message, separator, source.InnerException.GetInnerMessage(separator));
            }
        }
    }
}