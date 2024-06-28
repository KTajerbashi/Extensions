namespace Application.Layer.Model.Base.Commands;

public interface ICommandDispatcher
{
    Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command) where TCommand : class, ICommand<TData>;

}
