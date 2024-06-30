namespace UserAccessManagement.Application.Base;

public interface ICommandResult
{
    bool Success { get; }

    string Message { get; }
}