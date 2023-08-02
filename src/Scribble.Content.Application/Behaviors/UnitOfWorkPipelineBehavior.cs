using MediatR;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Behaviors;

public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkPipelineBehavior(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken token)
    {
        if (IsNotCommand())
            return await next();
        
        var response = await next();

        await _unitOfWork.SaveChangesAsync(token)
            .ConfigureAwait(false);

        return response;
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}