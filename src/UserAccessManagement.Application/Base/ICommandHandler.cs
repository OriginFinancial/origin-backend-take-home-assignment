namespace UserAccessManagement.Application.Base;

public interface ICommandHandler<T, TResult> where T : ICommand<TResult> where TResult : ICommandResult
{
    Task<TResult> HandleAsync(T command, CancellationToken cancellationToken = default);
}
