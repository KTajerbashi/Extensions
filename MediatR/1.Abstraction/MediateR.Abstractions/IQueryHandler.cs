using MediatR;

namespace MediateR.Abstractions;

public interface IQuery : IRequest
{

}
public interface IQuery<TResult> : IRequest<TResult>
{

}
public interface IQueryHandler<TQuery> : IRequestHandler<TQuery>
    where TQuery : IQuery
{
}
public interface IQueryHandler<TQuery, TData> : IRequestHandler<TQuery, TData>
    where TQuery : IQuery<TData>
{
}
