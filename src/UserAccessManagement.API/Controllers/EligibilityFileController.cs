using Microsoft.AspNetCore.Mvc;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;

namespace UserAccessManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibilityFileController : ControllerBase
{
    private readonly ICommandHandler<AddEligibilityFileCommand, CommandResult> _addEligibilityFileCommandHandler;

    public EligibilityFileController(ICommandHandler<AddEligibilityFileCommand, CommandResult> addEligibilityFileCommandHandler)
    {
        _addEligibilityFileCommandHandler = addEligibilityFileCommandHandler;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy" });
    }

    [HttpPost]
    public async Task<IActionResult> AddElibilityFileAsync([FromBody] AddEligibilityFileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _addEligibilityFileCommandHandler.HandleAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}