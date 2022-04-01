using Microsoft.AspNetCore.Mvc;
using ResultHandler.Exceptions;
using ResultHandlerTests.Unit.Doubles.FakeObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResultHandlerTests.Unit.Doubles;

[ApiController]
[Route("[controller]/[action]")]
public class FakeController : ControllerBase
{
    private readonly List<Person> _people;

    public FakeController()
    {
        _people = new List<Person>
        {
            new Person{ Id = 1, FirstName = "Arash",LastName="Abbac",IsActive = true},
            new Person{ Id = 2, FirstName = "Arash2",LastName="Abbac2",IsActive = false},
        };
    }

    [HttpGet("{id?}")]
    public IActionResult Get(int? id)
    {
        if (id is null)
            return BadRequest("Id is required!");

        var person = _people.Find(x => x.Id == id);

        if(person is null)
            return NotFound("Person is not found!");

        return Ok(person);
    }

    [HttpPost]
    public IActionResult
        Post(Person person)
    {
        return Ok(person);
    }

    [HttpPatch("{id?}")]
    public IActionResult ChangeActivity(int? id)
    {
        if (id is null)
            return BadRequest();

        var person = _people.Find(x => x.Id == id);

        if (person is null)
            return NotFound();

        person.IsActive = person.IsActive != true;

        return Ok();
    }
}
