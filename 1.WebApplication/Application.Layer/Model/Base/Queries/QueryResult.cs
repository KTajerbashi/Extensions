using Application.Layer.Model.Base.Entities;

namespace Application.Layer.Model.Base.Queries;

public sealed class QueryResult<TData> : ApplicationServiceResult
{
    public TData? _data;
    public TData? Data
    {
        get
        {
            return _data;
        }
    }
}
