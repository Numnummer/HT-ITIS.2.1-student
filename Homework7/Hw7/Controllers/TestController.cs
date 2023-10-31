using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hw7.Controllers
{
    public class TestController : Controller
    {
        // GET: TestController1
        public ActionResult TestModel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestModel(TestModel testModel)
        {
            return View(testModel);
        }

        public ActionResult UnsupportedModel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnsupportedModel(UnsupportedModel unsopportedModel)
        {
            return View(unsopportedModel);
        }
    }
}
