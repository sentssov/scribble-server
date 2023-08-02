using AutoMapper;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Queries;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagResponse>()
            .ForMember(x => x.Id, i => i.MapFrom(m => m.Id))
            .ForMember(x => x.UserId, i => i.MapFrom(m => m.UserId))
            .ForMember(x => x.Name, i => i.MapFrom(m => m.Name.Value));
    }
}