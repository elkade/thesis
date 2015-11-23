using AutoMapper;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public static class AutoMapperServiceConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Page, PageDto>()
                .ForMember(dto => dto.Parent, conf => conf.MapFrom(p => p.Parent == null ? null : new ParentDto { Title = p.Parent.Title, UrlName = p.Parent.UrlName }));
            Mapper.CreateMap<MenuItem, MenuItemDto>()
                .ForMember(dto => dto.PageName, conf => conf.MapFrom(p => p.Page.UrlName))
                .ForMember(dto => dto.Title, conf => conf.MapFrom(p => p.Page.Title));
        }
    }
}
