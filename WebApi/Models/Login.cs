using FluentValidation;

namespace WebApi.Models;

public class Login
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username or email are required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
