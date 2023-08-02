using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Queries.GetGroupById;

public class GetGroupByIdQuery : IQuery<GroupResponse>
{
    public GetGroupByIdQuery(GroupId groupId) => 
        GroupId = groupId;
    public GroupId GroupId { get; }
}