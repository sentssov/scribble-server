using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Queries.GetTagsCount;

public class GetTagsCountQuery : IQuery<long>
{
    public GetTagsCountQuery(UserId userId) => UserId = userId;

    public UserId UserId { get; }
}