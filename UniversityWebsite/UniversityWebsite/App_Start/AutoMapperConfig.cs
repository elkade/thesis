using AutoMapper;
using UniversityWebsite.Domain;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new PageProfile()));
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
                Name = p.Title
            });
            Mapper.CreateMap<PageViewModel, Page>().ConvertUsing(vm => new Page
            {
                Content = vm.Content,
                Language = new Language{CountryCode = vm.Language},
                Title = vm.Name
            });
        }
    }
}