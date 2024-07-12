using MediatR;

namespace MediateR.Abstractions;

public interface ICommandDispatcher
{
}


public interface ICommand : IRequest
{

}
public interface ICommand<TResult> : IRequest<TResult>
{

}
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}
public interface ICommandHandler<TCommand, TData> : IRequestHandler<TCommand, TData>
    where TCommand : ICommand<TData>
{
}