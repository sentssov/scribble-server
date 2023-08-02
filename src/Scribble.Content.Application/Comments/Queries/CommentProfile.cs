using AutoMapper;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Queries;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentResponse>()
            .ForMember(x => x.Id, i => i.MapFrom(m => m.Id))
            .ForMember(x => x.UserId, i => i.MapFrom(m => m.UserId))
            .ForMember(x => x.PostId, i => i.MapFrom(m => m.PostId))
            .ForMember(x => x.Text, i => i.MapFrom(m => m.Text.Value));
    }
}