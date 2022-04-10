using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PeopleController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] Models.Person person)
    {
        return Ok(person);
    }

    [HttpPost]
    public IActionResult Login([FromBody] Models.Login login)
    {
        return Ok(login);
    }
}
