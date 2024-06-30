using System.Net;

namespace UserAccessManagement.API.ActionResults;

public record ErrorResult(HttpStatusCode StatusCode, string Message, string? DetailedMessage);