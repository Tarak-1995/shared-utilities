using Microsoft.AspNetCore.Mvc;

namespace PrimeroEdge.SharedUtilities.Api.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Ok("Welcome to SharedUtilities APIs.");
        }
    }
}