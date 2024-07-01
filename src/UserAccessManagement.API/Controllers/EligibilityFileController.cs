using Microsoft.AspNetCore.Mvc;
using System.Text;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;

namespace UserAccessManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibilityFileController : ControllerBase
{
    private readonly ICommandHandler<AddEligibilityFileCommand, CommandResult> _addEligibilityFileCommandHandler;
    private readonly ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult> _getLastElibilityFileByEmployerCommandHandler;
    private readonly ICommandHandler<GetLastElibilityFileReportByEmployerCommand, GetLastElibilityFileReportByEmployerCommandResult> _getLastElibilityFileReportByEmployerCommandHandler;

    public EligibilityFileController(ICommandHandler<AddEligibilityFileCommand, CommandResult> addEligibilityFileCommandHandler, ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult> getLastElibilityFileByEmployerCommandHandler, ICommandHandler<GetLastElibilityFileReportByEmployerCommand, GetLastElibilityFileReportByEmployerCommandResult> getLastElibilityFileReportByEmployerCommandHandler)
    {
        _addEligibilityFileCommandHandler = addEligibilityFileCommandHandler;
        _getLastElibilityFileByEmployerCommandHandler = getLastElibilityFileByEmployerCommandHandler;
        _getLastElibilityFileReportByEmployerCommandHandler = getLastElibilityFileReportByEmployerCommandHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetLastElibilityFileByEmployerAsync([FromQuery] string employerName, CancellationToken cancellationToken = default)
    {
        var result = await _getLastElibilityFileByEmployerCommandHandler.HandleAsync(new GetLastElibilityFileByEmployerCommand(employerName), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddElibilityFileAsync([FromBody] AddEligibilityFileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _addEligibilityFileCommandHandler.HandleAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("/report")]
    public async Task<IActionResult> GetLastElibilityFileReportByEmployerAsync([FromQuery] string employerName, CancellationToken cancellationToken = default)
    {
        var result = await _getLastElibilityFileReportByEmployerCommandHandler.HandleAsync(new GetLastElibilityFileReportByEmployerCommand(employerName), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

        streamWriter.WriteLine("Content,Status");

        foreach (var line in result.ElibilityFileLines)
        {
            streamWriter.WriteLine($"{line.Content},{line.Status}");
        }

        string csvContentType = "text/csv";
        string csvFileName = $"{employerName}_eligibility_file_processed.csv";

        return File(memoryStream.ToArray(), csvContentType, csvFileName);
    }
}