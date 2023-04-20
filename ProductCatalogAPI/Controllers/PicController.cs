using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicController : ControllerBase
    { 
        // Declare a const (readonly) variable in the global scope for easy access.
        private readonly IWebHostEnvironment _env;
        // In order to make this controller effective, you need access to the environment where this microservice is hosted
        public PicController(IWebHostEnvironment env) 
        {
            _env = env;
        }
        // Define the route of your method by binding the parameter.
        [HttpGet("{id}")]
        // Create an API that selects the image produced when passed in the id parameter
        public IActionResult GetImage(int id) 
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine($"{webRoot}/Pics/", $"Ring{id}.jpg");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/jpeg");
        }

    }
}
