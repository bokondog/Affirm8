using Microsoft.AspNetCore.Mvc;

namespace KindWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Kind Words API is working!", timestamp = DateTime.UtcNow });
        }
        
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", service = "KindWords API" });
        }
    }
} 