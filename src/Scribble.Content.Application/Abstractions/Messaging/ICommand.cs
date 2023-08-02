using MediatR;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}