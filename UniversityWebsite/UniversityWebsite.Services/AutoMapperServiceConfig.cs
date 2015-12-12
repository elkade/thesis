using System.Collections.Generic;
using AutoMapper;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Odpowiada za konfigurację narzędzia do automatycznego mapowania klas AutoMapper
    /// </summary>
    public static class AutoMapperServiceConfig
    {
        /// <summary>
        /// Wykonuje konfigurację AutoMappera
        /// </summary>
        public static void Configure()
        {
            Mapper.CreateMap<Page, PageDto>()
                .ForMember(dto => dto.Parent, conf => conf.MapFrom(p => p.Parent == null ? null : new ParentDto { Title = p.Parent.Title, UrlName = p.Parent.UrlName }));
            Mapper.CreateMap<MenuItem, MenuItemDto>()
                .ForMember(dto => dto.UrlName, conf => conf.MapFrom(p => p.Page.UrlName))
                .ForMember(dto => dto.Title, conf => conf.MapFrom(p => p.Page.Title));
            Mapper.CreateMap<Menu, MenuDto>()
                .ForMember(dto => dto.Items, conf => conf.MapFrom(p => p.Items));
            Mapper.CreateMap<Page, PageMenuItem>().ForMember(dto => dto.Children, conf => conf.MapFrom(p => new List<PageMenuItem>()));

            Mapper.CreateMap<Subject, SubjectDto>();

            Mapper.CreateMap<Article, ArticleDto>();
        }
    }
}
