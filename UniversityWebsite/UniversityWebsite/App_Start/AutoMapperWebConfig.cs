using System.Linq;
using AutoMapper;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Model;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services.Model;
using UniversityWebsite.ViewModels;
using News = UniversityWebsite.Api.Model.Teaching.News;

namespace UniversityWebsite
{
    /// <summary>
    /// Odpowiada za konfigurację narzędzia do automatycznego mapowania klas AutoMapper
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Wykonuje konfigurację AutoMappera
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new PageProfile());
                cfg.AddProfile(new MenuProfile());
                cfg.AddProfile(new SubjectProfile());
                cfg.AddProfile(new LanguageProfile());
            });
        }
    }
    /// <summary>
    /// Odpowiada cz część konfiguracji AutoMappera dotyczącą stron
    /// </summary>
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
            Mapper.CreateMap<PageMenuItem, PageMenuItemVm>().ConvertUsing(p => new PageMenuItemVm
            {
                Title = p.Title,
                UrlName = p.UrlName,
                Children = p.Children.Select(c=>new PageMenuItemVm{Title = c.Title, UrlName = c.UrlName}).ToList()
            });
        }
    }
    /// <summary>
    /// Odpowiada cz część konfiguracji AutoMappera dotyczącą przedmiotów
    /// </summary>
    public class SubjectProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subject, SubjectVm>().ConvertUsing(p => new SubjectVm
            {
                Name = p.Name,
                //Files = new FilesSectionVm(),
                //News = string.Join("</br>", p.News.ToString()),
                //Syllabus = p.Syllabus.ToString(),
                //Schedule = p.Schedule.ToString(),

                //SemesterNumber = p.Semester.Number,
            });

            Mapper.CreateMap<News, NewsDto>().ConvertUsing(p => new NewsDto
            {
                Content = p.Content,
                Header = p.Header
            });

            Mapper.CreateMap<SubjectPost, SubjectDto>().ConvertUsing(p => new SubjectDto
            {
                Name = p.Name,
                Schedule = new ArticleDto{Content = p.Schedule.Content},
                Syllabus = new ArticleDto{Content = p.Syllabus.Content},
                Semester = p.Semester,
            });

            Mapper.CreateMap<SubjectPut, SubjectDto>().ConvertUsing(p => new SubjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Schedule = new ArticleDto { Content = p.Schedule.Content },
                Syllabus = new ArticleDto { Content = p.Syllabus.Content },
                Semester = p.Semester,
            });
        }
    }
    /// <summary>
    /// Odpowiada cz część konfiguracji AutoMappera dotyczącą menu
    /// </summary>
    public class MenuProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<MenuItemDto, MenuItemViewModel>();
            Mapper.CreateMap<MenuDto, MainMenuViewModel>();
            Mapper.CreateMap<Tile, TileViewModel>();
        }
    }

    /// <summary>
    /// Odpowiada cz część konfiguracji AutoMappera dotyczącą języków
    /// </summary>
    public class LanguageProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewLanguage, DictionaryDto>();
        }
    }
}