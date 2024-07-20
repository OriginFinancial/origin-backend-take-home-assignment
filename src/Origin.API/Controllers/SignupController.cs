using Application.Messages.Commands;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Origin.API.Controllers;


[ApiController]
[Route("api/signup")]
public class SignupController : ControllerBase
{
    private readonly IMediator _mediator;

    public SignupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SignupModel model)
    {
        await _mediator.Send(new ProcessSignupCommand(model.Email, model.Password, model.Country));
        return Ok();
    }
}