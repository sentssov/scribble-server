using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQuery : IQuery<CommentResponse>
{
    public GetCommentByIdQuery(CommentId commentId) => 
        CommentId = commentId;

    public CommentId CommentId { get; }
}