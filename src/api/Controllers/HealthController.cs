using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace r7
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Healthy");
        }
    }
}