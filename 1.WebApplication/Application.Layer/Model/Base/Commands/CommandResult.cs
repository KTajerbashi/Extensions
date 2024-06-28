using Application.Layer.Model.Base.Entities;

namespace Application.Layer.Model.Base.Commands;

public class CommandResult : ApplicationServiceResult
{

}
public class CommandResult<TData> : CommandResult
{
    public TData? _data;
    public TData? Data => _data;
}