using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Queries.GetPostById;

public class GetPostByIdQuery : IQuery<PostResponse>
{
    public GetPostByIdQuery(PostId postId) => 
        PostId = postId;

    public PostId PostId { get; }
}