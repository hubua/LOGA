using Microsoft.AspNetCore.Mvc;

namespace LOGA.WebUI.Controllers
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