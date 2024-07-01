namespace UserAccessManagement.Application.Base;

public interface ICommand<TResult> where TResult : ICommandResult
{
    string? ValidationMessages { get; }

    bool Validate();
}