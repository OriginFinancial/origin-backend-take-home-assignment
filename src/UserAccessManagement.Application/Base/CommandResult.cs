namespace UserAccessManagement.Application.Base;

public record CommandResult(bool Success, string Message) : ICommandResult;