using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleController : ControllerBase
    {

        private readonly ILogger<SimpleController> _logger;

        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
        }
        [HttpGet("ThrowException")]
        public IActionResult ThrowException()
        {
            throw new SimpleException("test");
        }
        [HttpGet("Success")]
        public IActionResult Success()
        {
            return Ok("Ok");
        }
    }
}