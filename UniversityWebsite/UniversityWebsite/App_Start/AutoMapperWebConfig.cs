﻿using AutoMapper;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Model;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services.Model;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new PageProfile());
                cfg.AddProfile(new MenuProfile());
                cfg.AddProfile(new SubjectProfile());
            });
        }
    }

    public class PageProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PageDto, PageViewModel>().ConvertUsing(p => new PageViewModel
            {
                Content = p.Content,
                CountryCode = p.CountryCode,
                Title = p.Title,
                LastUpdateDate = p.LastUpdateDate,
                CreationDate = p.CreationDate
            });
            Mapper.CreateMap<PageViewModel, PageDto>().ConvertUsing(vm => new PageDto
            {
                Content = vm.Content,
                CountryCode = vm.CountryCode,
                Title = vm.Title,
                LastUpdateDate = vm.LastUpdateDate,
                CreationDate = vm.CreationDate
            });
            Mapper.CreateMap<PagePut, PageDto>().ConvertUsing(p => new PageDto
            {
                Content = p.Content,
                CountryCode = p.CountryCode,
                Title = p.Title,
                GroupId = p.GroupId,
                Id = p.Id,
                Parent = p.ParentId==null?null:new ParentDto{Id = p.ParentId.Value},
                UrlName = p.UrlName
            });
            Mapper.CreateMap<PagePosted, PageDto>().ConvertUsing(p => new PageDto
            {
                Content = p.Content,
                CountryCode = p.CountryCode,
                Title = p.Title,
                GroupId = p.GroupId,
                Parent = p.ParentId == null ? null : new ParentDto { Id = p.ParentId.Value },
                UrlName = p.UrlName,
            });
        }
    }

    public class SubjectProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subject, SubjectViewModel>().ConvertUsing(p => new SubjectViewModel
            {
                Name = p.Name,
            });
            Mapper.CreateMap<SubjectViewModel, Subject>().ConvertUsing(vm => new Subject
            {
                Name = vm.Name,
            });
        }
    }

    public class MenuProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<MenuItemDto, MenuItemViewModel>();
            Mapper.CreateMap<MenuDto, MainMenuViewModel>();
        }
    }
}