using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Queries.GetTagById;

public class GetTagByIdQuery : IQuery<TagResponse>
{
    public GetTagByIdQuery(TagId tagId) => 
        TagId = tagId;
    
    public TagId TagId { get; }
}