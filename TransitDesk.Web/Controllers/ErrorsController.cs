using Microsoft.AspNetCore.Mvc;

namespace TransitDesk.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Errors/Status")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Status(int code)
        {
            Response.StatusCode = code;

            ViewBag.StatusCode = code;

            return code switch
            {
                404 => View("NotFound"),
                403 => View("Forbidden"),
                _   => View("Default")
            };
        }
    }
}