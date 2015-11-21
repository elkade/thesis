using AutoMapper;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public static class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.CreateMap<Page, PageDto>()
                .ForMember(dto => dto.Parent, conf => conf.MapFrom(p => p.Parent == null ? null : new ParentDto { Title = p.Parent.Title, UrlName = p.Parent.UrlName }));
        }
    }
}
