using Microsoft.AspNetCore.Mvc;

namespace LOGAWebApp.Controllers
{
    public class DownloadsController : Controller
    {

        public DownloadsController()
        {
            ViewData["ActiveMenu"] = "Downloads"; //TODO Highlight active item in UI
        }
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}