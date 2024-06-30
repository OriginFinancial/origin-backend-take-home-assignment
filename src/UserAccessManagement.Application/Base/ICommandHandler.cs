namespace UserAccessManagement.Application.Base;

public interface ICommandHandler<T, TResult> where T : ICommand where TResult : ICommandResult
{
    Task<TResult> HandleAsync(T command, CancellationToken cancellationToken = default);
}
