using FluentResults;
using MediatR;

namespace Architecture.Api.Abstractions
{
    public interface IQuery<T> : IRequest<Result<T>>;

    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
}