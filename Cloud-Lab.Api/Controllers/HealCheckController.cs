using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealCheckController : Controller
    {
        [HttpGet]
        public IActionResult Check()
        {
            return NoContent();
        }
    }
}