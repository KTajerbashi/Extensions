namespace WebApplicationAPI.Model.Base;

public interface ICommand
{
}
public interface ICommand<TData>
{
}
public interface ICommandDispatcher
{
    Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command) where TCommand : class, ICommand<TData>;

}
public class CommandResult : ApplicationServiceResult
{

}
public class CommandResult<TData> : CommandResult
{
    public TData? _data;
    public TData? Data => _data;
}