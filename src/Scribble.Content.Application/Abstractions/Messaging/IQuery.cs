using MediatR;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}