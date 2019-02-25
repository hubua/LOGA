using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{

    /*
    public static class MVCTestExtention
    {
        public static T IsActionResultOfType<T>(this ActionResult actionResult) where T : ActionResult
        {
            Assert.IsInstanceOfType(actionResult, typeof(T));
            return (T)actionResult;
        }

        public static ViewResult IsViewResult(this ActionResult actionResult)
        {
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult), $"'{actionResult.GetType().ToString()}' is not a ViewResult.");
            return (ViewResult)actionResult;
        }

        public static ViewResult HasViewName(this ViewResult viewResult, string viewName)
        {
            Assert.IsTrue(viewResult.ViewName == viewName, $"View name does not match: '{viewResult.ViewName}' instead of '{viewName}'.");
            return viewResult;
        }

        public static ViewResult HasModel<T>(this ViewResult viewResult)
        {
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(T));
            return viewResult;
        }

        public static RedirectToRouteResult RedirectsTo(this RedirectToRouteResult redirectResult, string actionName)
        {
            Assert.AreEqual(redirectResult.RouteValues["Action"], actionName);
            return redirectResult;
        }

        public static RedirectToRouteResult WithParam(this RedirectToRouteResult redirectResult, string paramName, string paramValue)
        {
            Assert.IsTrue(redirectResult.RouteValues.Keys.ToList().Exists(item => item == paramName), $"Parameter '{paramName}' does not exist.");
            Assert.AreEqual(redirectResult.RouteValues[paramName], paramValue);
            return redirectResult;
        }

        public static RedirectToRouteResult WithParam(this RedirectToRouteResult redirectResult, string paramName, int paramValue)
        {
            Assert.IsTrue(redirectResult.RouteValues.Keys.ToList().Exists(item => item == paramName), $"Parameter '{paramName}' does not exist.");
            Assert.AreEqual(redirectResult.RouteValues[paramName], paramValue);
            return redirectResult;
        }
    }
    */
    [TestClass]
    public class Controller_Test
    {
        /*
        [TestMethod]
        public void Home_Index_Test()
        {
            var controller = new LOGAWebApp.Controllers.HomeController();
            controller.Index().IsViewResult().HasViewName("Index");
        }

        [TestMethod]
        public void Learn_Index_Test()
        {
            var controller = new LOGAWebApp.Controllers.LearnController();
            var actionresult = controller.Index();

            actionresult.IsActionResultOfType<ViewResult>().HasViewName("Index");

            var model = (Dictionary<char, LOGAWebApp.Models.GeorgianLetter>)((ViewResult)actionresult).Model;

            const int INVALID_LEARN_INDEX = 1000;
            const int VALID_LEARN_INDEX = 2;

            var c = new LOGAWebApp.Controllers.LearnController();
            c.Letter(INVALID_LEARN_INDEX).IsActionResultOfType<RedirectToRouteResult>().RedirectsTo("Letter").WithParam("lid", 1);
            c.Letter(VALID_LEARN_INDEX).IsActionResultOfType<ViewResult>().HasViewName("Letter").HasModel<LOGAWebApp.Models.GeorgianLetter>();
        }
        */



    }

}