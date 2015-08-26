using System.Web.Mvc;
using Homepage.Config;
using Homepage.Factories;
using Homepage.Managers;
using Newtonsoft.Json;
using System.IO;

namespace Homepage.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            Configuration config = JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText(Server.MapPath("~/config.json")));
            config.BasePath = Server.MapPath("~/");

            var model = new ImageIndexViewModelFactory().BuildImageIndexViewModel(config);
            return View(model);
        }

        public bool Update()
        {
            new ThumbnailManager().EnsureThumbnailsUpToDate();
            return true;
        }
    }
}