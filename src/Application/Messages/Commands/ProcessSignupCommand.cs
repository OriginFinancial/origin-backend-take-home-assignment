using MediatR;

namespace Application.Messages.Commands;

public class ProcessSignupCommand : IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Country { get; set; }
    
    public ProcessSignupCommand(string email, string password, string country)
    {
        Email = email;
        Password = password;
        Country = country;
    }

}