using AutoMapper;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Queries;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupResponse>()
            .ForMember(x => x.Id, i => i.MapFrom(m => m.Id))
            .ForMember(x => x.UserId, i => i.MapFrom(m => m.UserId))
            .ForMember(x => x.Name, i => i.MapFrom(m => m.Name.Value))
            .ForMember(x => x.ShortName, i => i.MapFrom(m => m.ShortName.Value))
            .ForMember(x => x.Description, i => i.MapFrom(m => m.Description.Value))
            .ForMember(x => x.CreatedOnUtc, i => i.MapFrom(m => m.CreatedOnUtc))
            .ForMember(x => x.ModifiedOnUtc, i => i.MapFrom(m => m.ModifiedOnUtc));
    }
}