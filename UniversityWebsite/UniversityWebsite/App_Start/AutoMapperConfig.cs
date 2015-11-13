using AutoMapper;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
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
            Mapper.CreateMap<Page, PageViewModel>().ConvertUsing(p=>new PageViewModel
            {
                Content = p.Content,
                Language = p.Language.CountryCode,
                Name = p.Title,
                LastUpdateDate = p.LastUpdateDate,
                CreationDate = p.CreationDate
            });
            Mapper.CreateMap<PageViewModel, Page>().ConvertUsing(vm => new Page
            {
                Content = vm.Content,
                Language = new Language{CountryCode = vm.Language},
                Title = vm.Name,
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