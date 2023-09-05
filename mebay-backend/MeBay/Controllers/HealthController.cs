using Microsoft.AspNetCore.Mvc;

namespace MeBay.UserController
{
    [ApiController]
    [Route("/[controller]")]
    public class HealthController : ControllerBase
    {
        public HealthController() { }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok(new { health = "ok" });
        }
    }
}
