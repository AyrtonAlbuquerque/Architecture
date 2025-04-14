using FluentResults;
using MediatR;

namespace Architecture.Api.Abstractions
{
    public interface ICommand : IRequest<Result>;

    public interface ICommand<T> : IRequest<Result<T>>;

    public interface ICommandHandler<T> : IRequestHandler<T, Result> where T : ICommand;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
}