using FluentResults;
using MediatR;

namespace Architecture.Api.Abstractions
{
    public interface IBase;
    public interface IBase<T>;
    public interface IQuery : IBase, IRequest<Result>;
    public interface IQuery<T> : IBase<T>, IRequest<Result<T>>;
    public interface ICommand : IBase, IRequest<Result>;
    public interface ICommand<T> : IBase<T>, IRequest<Result<T>>;
    public interface IHandler<TRequest> : IRequestHandler<TRequest, Result> where TRequest : IRequest<Result>, IBase;
    public interface IHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>> where TRequest : IRequest<Result<TResponse>>, IBase<TResponse>;
}