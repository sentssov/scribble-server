using Scribble.Content.Application.Abstractions.Messaging;

namespace Scribble.Content.Application.Groups.Queries.GetGroupByShortName;

public class GetGroupByShortNameQuery : IQuery<GroupResponse>
{
    public GetGroupByShortNameQuery(string shortName) => 
        ShortName = shortName;

    public string ShortName { get; set; }
}