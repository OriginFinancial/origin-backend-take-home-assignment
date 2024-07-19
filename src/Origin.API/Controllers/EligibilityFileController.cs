using Application.Messages.Commands;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Origin.API.Controllers;

[ApiController]
[Route("api/eligibility")]
public class EligibilityFileController : ControllerBase
{
    private readonly IMediator _mediator;

    public EligibilityFileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    public async Task<ActionResult<EligibilityProcessResult>> ProcessEligibilityFile([FromBody] EligibilityFile model)
    {
        var result = await _mediator.Send(new ProcessEligibilityFileCommand(model.File, model.EmployerName));
        return Ok(result);
    }
}