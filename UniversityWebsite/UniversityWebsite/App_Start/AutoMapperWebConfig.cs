﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Api.Model.Users;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Model;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services.Model;

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
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new RequestProfile());
            });
        }
    }
    /// <summary>
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą stron
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
                CreationDate = p.CreationDate,
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
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą przedmiotów
    /// </summary>
    public class SubjectProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<News, NewsVm>().ConvertUsing(p => new NewsVm
            {
                Author = p.Author==null?string.Empty:p.Author.UserName,
                Content = p.Content,
                Header = p.Header,
                PublishDate = p.PublishDate
            });

            Mapper.CreateMap<File, FileDto>().ConvertUsing(p => new FileDto
            {
                Created = p.UploadDate,
                Id = p.Id,
                Name = p.FileName,
                Modified = p.UpdateDate,
                Version = p.Version
            });

            Mapper.CreateMap<Subject, SubjectVm>().ConvertUsing(p => new SubjectVm
            {
                Name = p.Name,
                UrlName = p.UrlName,
                Files = Mapper.Map<List<FileDto>>(p.Files) ?? new List<FileDto>(),
                News = Mapper.Map<List<NewsVm>>(p.News.OrderByDescending(n => n.PublishDate)),
                Syllabus = p.Syllabus==null?string.Empty:p.Syllabus.Content,
                Schedule = p.Schedule==null?string.Empty:p.Schedule.Content,

                //SemesterNumber = p.Semester.Number,
            });

            Mapper.CreateMap<NewsPost, NewsDto>().ConvertUsing(p => new NewsDto
            {
                Content = p.Content,
                Header = p.Header
            });

            Mapper.CreateMap<NewsPut, NewsDto>().ConvertUsing(p => new NewsDto
            {
                Id = p.Id,
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
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą menu
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
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą użytkowników
    /// </summary>
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserVm, User>().ConvertUsing(p => new User
            {
                Id = p.Id,
                Email = p.Email,
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName = p.Email,
                Pesel = p.Pesel,
                IndexNumber = p.IndexNumber
            });

            Mapper.CreateMap<User, UserVm>().ConvertUsing(p => new UserVm
            {
                Email = p.Email,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Pesel = p.Pesel,
                IndexNumber = p.IndexNumber,
                Id = p.Id,
            });
        }
    }

    /// <summary>
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą języków
    /// </summary>
    public class LanguageProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewLanguage, DictionaryDto>();
        }
    }

    /// <summary>
    /// Odpowiada za część konfiguracji AutoMappera dotyczącą wniosków o zapisanie na przedmiot
    /// </summary>
    public class RequestProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SignUpRequest, RequestVm>().ConvertUsing(r=>new RequestVm
            {
                Id = r.Id,
                StudentFirstName = r.Student.FirstName,
                StudentLastName = r.Student.LastName,
                StudentId = r.StudentId,
                StudentIndex = r.Student.IndexNumber,
                SubjectTitle = r.Subject.Name,
                SubjectUrlName = r.Subject.UrlName,
                SubjectId = r.SubjectId,
                Status = r.Status.ToString()
            });
        }
    }
}