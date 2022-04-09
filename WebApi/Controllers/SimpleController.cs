using EasyResult;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SimpleController : ControllerBase
    {

        private readonly ILogger<SimpleController> _logger;
   

        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
         
        }

        [HttpGet]
        public IActionResult ThrowException()
        {
            throw new SimpleException("test");
        }

        [HttpGet]
        public IActionResult UnhandledException()
        {
            throw new Exception("unhandled exception");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return Ok(new { FirstName = "Amar" , LastName = "Potki" });
        }

        [HttpGet]
        public IActionResult Fail ()
        {
            return BadRequest("Failed process");
        }

        [HttpGet]
        public IActionResult SuccessResult()
        {
            var obj = new Result().WithSuccess();
            return Ok(obj);
        }

        [HttpGet]
        public IActionResult SuccessResultWithData()
        {
            var obj = new Result<object>()
                .WithSuccess()
                .WithData(new { FirstName = "Amar", LastName = "Potki" });

            return Ok(obj);
        }

        [HttpGet]
        public IActionResult SuccessResultWithDataAndCustomMessage()
        {
            var obj = new Result<object>()
                .WithSuccess("Operation succeeded!")
                .WithData(new { FirstName = "Amar", LastName = "Potki" });

            return Ok(obj);
        }
    }
}