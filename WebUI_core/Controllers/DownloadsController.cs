using Microsoft.AspNetCore.Mvc;

namespace LOGA.WebUI.Controllers
{
    public class DownloadsController : Controller
    {

        public DownloadsController()
        {
            ViewData["ActiveMenu"] = "Downloads"; // TODO
        }
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}