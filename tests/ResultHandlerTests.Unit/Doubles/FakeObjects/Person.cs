using System.ComponentModel.DataAnnotations;

namespace ResultHandlerTests.Unit.Doubles.FakeObjects;

public class Person
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public bool IsActive { get; set; }
}
