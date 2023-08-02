using AutoMapper;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Queries;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostResponse>()
            .ForMember(x => x.Id, i => i.MapFrom(m => m.Id))
            .ForMember(x => x.GroupId, i => i.MapFrom(m => m.GroupId))
            .ForMember(x => x.Title, i => i.MapFrom(m => m.Title.Value))
            .ForMember(x => x.Content, i => i.MapFrom(m => m.Content.Value))
            .ForMember(x => x.Description, i => i.MapFrom(m => m.Description.Value))
            .ForMember(x => x.CreatedOnUtc, i => i.MapFrom(m => m.CreatedOnUtc))
            .ForMember(x => x.ModifiedOnUtc, i => i.MapFrom(m => m.ModifiedOnUtc));
    }
}