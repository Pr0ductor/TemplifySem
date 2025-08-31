using Microsoft.AspNetCore.Mvc;

namespace Templify.mvc.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }

        [Route("Error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
