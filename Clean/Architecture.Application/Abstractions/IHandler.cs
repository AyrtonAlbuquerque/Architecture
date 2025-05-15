using FluentResults;
using MediatR;

namespace Architecture.Application.Abstractions
{
    public interface IQuery : IRequest<Result>;
    public interface IQuery<T> : IRequest<Result<T>>;
    public interface ICommand : IRequest<Result>;
    public interface ICommand<T> : IRequest<Result<T>>;
    public interface IHandler<TRequest> : IRequestHandler<TRequest, Result> where TRequest : IRequest<Result>;
    public interface IHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>> where TRequest : IRequest<Result<TResponse>>;
}