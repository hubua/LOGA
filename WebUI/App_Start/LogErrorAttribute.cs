using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGA.WebUI
{
    public class LogErrorAttribute : HandleErrorAttribute
    {

        /*
        private readonly string logaConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private const string SQL_QUERY =
            @"INSERT INTO ErrorLog
            (
                Id, [InnerMessage], [StackTrace], [Controller], [Action], [UserIP], [CreatedDate]
            )
            VALUES
            (
                1, @InnerMessage,  @StackTrace, @Controller, @Action, @UserIP, @CreatedDate
            )";
         */

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var userIP = filterContext.HttpContext.Request.UserHostAddress;
            var date = DateTime.Now;
            var filename = $"error_{date.ToString("yyyyMMddHHmmss")}_{userIP.Replace('.', '_').Replace(':', '_')}.txt";
            using (var file = System.IO.File.CreateText(filterContext.HttpContext.Server.MapPath(@"~\App_Data\" + filename)))
            {
                file.WriteLine($"{date.ToLongTimeString()} - {userIP}");
                file.WriteLine(filterContext.Exception.GetInnerMessage());
                file.WriteLine(filterContext.Exception.StackTrace);
                //TODO: full date, controller in release mode
            }

            /*

            using (SqlConnection connection = new SqlConnection(logaConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_QUERY, connection))
                {
                    command.Parameters.AddWithValue("@InnerMessage", filterContext.Exception.GetInnerMessage());
                    command.Parameters.AddWithValue("@StackTrace", filterContext.Exception.StackTrace);
                    command.Parameters.AddWithValue("@Controller", filterContext.RouteData.Values["Controller"]);
                    command.Parameters.AddWithValue("@Action", filterContext.RouteData.Values["Action"]);
                    command.Parameters.AddWithValue("@UserIP", filterContext.HttpContext.Request.UserHostAddress);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
            */
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