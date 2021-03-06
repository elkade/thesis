﻿using System.Collections.Generic;
using System.Linq;
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
                .ForMember(dto => dto.Parent, conf => conf.MapFrom(p => p.Parent==null?null:new ParentDto()));
            Mapper.CreateMap<MenuItem, MenuItemDto>()
                .ForMember(dto => dto.UrlName, conf => conf.MapFrom(p => p.Page.UrlName))
                .ForMember(dto => dto.Title, conf => conf.MapFrom(p => p.Page.Title));
            Mapper.CreateMap<Menu, MenuDto>()
                .ForMember(dto => dto.Items, conf => conf.MapFrom(p => p.Items));
            Mapper.CreateMap<Page, PageMenuItem>().ForMember(dto => dto.Children, conf => conf.MapFrom(p => new List<PageMenuItem>()));

            Mapper.CreateMap<Subject, SubjectDto>();

            Mapper.CreateMap<News, NewsDto>()
                .ForMember(dto => dto.Author, conf => conf.MapFrom(p => p.Author.UserName));
            Mapper.CreateMap<Syllabus, ArticleDto>()
                .ForMember(dto => dto.Author, conf => conf.MapFrom(p => p.Author.UserName));
            Mapper.CreateMap<Schedule, ArticleDto>()
                .ForMember(dto => dto.Author, conf => conf.MapFrom(p => p.Author.UserName));


        }
    }
}
