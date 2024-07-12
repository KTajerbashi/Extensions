using MediateR.Abstractions;

namespace MediateR.Extensions.Options.Commands;

public abstract class CommandHandler<TRequest> : ICommandHandler<TRequest>
    where TRequest : ICommand
{
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class CommandHandler<TRequest, TData> : ICommandHandler<TRequest, TData>
    where TRequest : ICommand<TData>
{
    public abstract Task<TData> Handle(TRequest request, CancellationToken cancellationToken);
}
