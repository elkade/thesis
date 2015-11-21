using AutoMapper;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Model;
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
            Mapper.CreateMap<PageDto, PageViewModel>().ConvertUsing(p=>new PageViewModel
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
            Mapper.CreateMap<Services.Model.MenuItemDto, ViewModels.MenuItemViewModel>();
            Mapper.CreateMap<Services.Model.MenuDto, ViewModels.MenuViewModel>();
        }
    }
}