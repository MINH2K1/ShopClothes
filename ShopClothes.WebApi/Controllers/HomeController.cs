using Microsoft.AspNetCore.Mvc;

namespace ShopClothes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController: ControllerBase
    {
        [HttpGet("index.html")]
        public IActionResult Index()
        {
            return Ok("Hello World");
        }
    }
}
