using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hw7.Controllers
{
    public class UnsupportedTypeController : Controller
    {
        // GET: UnsupportedTypeController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UnsopportedModel unsopportedModel)
        {
            return View(unsopportedModel);
        }
    }
}
