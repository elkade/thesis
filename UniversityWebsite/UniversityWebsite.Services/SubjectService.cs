using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Enums;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface ISubjectService
    {
        IEnumerable<Subject> GetSemester(int number);
        Subject GetSubject(string name);
        IEnumerable<SubjectDto> GetSubjects(int offset, int limit);
        SubjectDto AddSubject(SubjectDto subject, string authorId);
        SubjectDto UpdateSubject(SubjectDto subject, string authorId);
        NewsDto AddNews(int subjectId, NewsDto newsDto, string authorId);
        IEnumerable<NewsDto> GetNews(int subjectId);
        IEnumerable<User> GetStudents(int subjectId, int limit, int offset);
        void DeleteNews(int subjectId, int newsId);
        void DeleteSubject(int subjectId);
        SignUpAction GetAvailableAction(string studentId, int subjectId);
        NewsDto UpdateNews(int subjectId, NewsDto newsDto);
        void SignUpForSubject(int subjectId, string userId);
    }
    public class SubjectService : ISubjectService
    {
        private readonly IDomainContext _context;
        private const int SameTitleSubjectsMaxNumber = 100;

        public SubjectService(IDomainContext context)
        {
            _context = context;
        }

        public SignUpAction GetAvailableAction(string studentId, int subjectId)
        {
            var request =
                _context.SignUpRequests.SingleOrDefault(r => r.StudentId == studentId && r.SubjectId == subjectId);
            if (request == null)
                return SignUpAction.Submit;
            if (request.Status == RequestStatus.Submitted)
                return SignUpAction.InProgress;
            if (request.Status == RequestStatus.Approved)
                return SignUpAction.Resign;
            if (request.Status == RequestStatus.Refused)
                return SignUpAction.Refused;
            return SignUpAction.None;
        }

        public IEnumerable<Subject> GetSemester(int number)
        {
            return _context.Subjects.Where(s => s.Semester == number);
        }

        public Subject GetSubject(string name)
        {
            var subject = _context.Subjects.SingleOrDefault(s => s.UrlName == name);
            return subject;
        }

        public IEnumerable<SubjectDto> GetSubjects(int offset, int limit)
        {
            if (limit < 0) return Enumerable.Empty<SubjectDto>();
            limit = limit > 50 ? 50 : limit;

            return
                _context.Subjects.OrderBy(s => s.Semester)
                    .ThenBy(s => s.Name)
                    .Skip(offset)
                    .Take(limit)
                    .ProjectTo<SubjectDto>();
        }

        public SubjectDto AddSubject(SubjectDto subject, string authorId)
        {
            var dbSubject = new Subject
            {
                Name = subject.Name,
                Schedule = subject.Schedule == null ? new Schedule { AuthorId = authorId } :
                    new Schedule { AuthorId = authorId, Content = subject.Schedule.Content, PublishDate = DateTime.Now },
                Semester = subject.Semester,
                Syllabus = subject.Syllabus == null ? new Syllabus { AuthorId = authorId } :
                    new Syllabus { AuthorId = authorId, Content = subject.Syllabus.Content, PublishDate = DateTime.Now },
                UrlName = PrepareUniqueUrlName(subject.UrlName),
            };
            var addedSubject = _context.Subjects.Add(dbSubject);
            _context.SaveChanges();
            return Mapper.Map<SubjectDto>(addedSubject);
        }

        public SubjectDto UpdateSubject(SubjectDto subject, string authorId)
        {
            Subject dbSubject = _context.Subjects.Find(subject.Id);
            if (dbSubject == null)
                throw new PropertyValidationException("subject.Id", "No subject with id: " + subject.Id + " in database.");

            dbSubject.Name = subject.Name;

            dbSubject.Schedule.AuthorId = authorId;
            dbSubject.Schedule.Content = subject.Schedule.Content;
            dbSubject.Schedule.PublishDate = DateTime.Now;

            dbSubject.Syllabus.AuthorId = authorId;
            dbSubject.Syllabus.Content = subject.Syllabus.Content;
            dbSubject.Syllabus.PublishDate = DateTime.Now;

            dbSubject.Semester = subject.Semester;
            dbSubject.UrlName = PrepareUniqueUrlName(subject.UrlName);

            _context.SetModified(dbSubject);
            _context.SaveChanges();
            return Mapper.Map<SubjectDto>(dbSubject);
        }

        public NewsDto AddNews(int subjectId, NewsDto newsDto, string authorId)
        {
            var dbNews = new News
            {
                AuthorId = authorId,
                Content = newsDto.Content,
                PublishDate = DateTime.Now,
                Header = newsDto.Header,
                SubjectId = subjectId
            };
            var addedNews = _context.News.Add(dbNews);
            _context.SaveChanges();
            return Mapper.Map<NewsDto>(addedNews);
        }

        public void DeleteNews(int subjectId, int newsId)
        {
            var news = _context.News.Find(newsId);
            if (subjectId != news.SubjectId)
                throw new PropertyValidationException("subjectId", "");
            _context.SetDeleted(news);
            _context.SaveChanges();
        }

        public void DeleteSubject(int subjectId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject with id: " + subjectId);
            var newsToDelete = subject.News.ToList();
            foreach (var news in newsToDelete)
                _context.SetDeleted(news);
            _context.SetDeleted(subject.Schedule);
            _context.SetDeleted(subject.Syllabus);
            _context.SetDeleted(subject);
            _context.SaveChanges();
        }

        public IEnumerable<NewsDto> GetNews(int subjectId)
        {
            return _context.News.Where(n => n.SubjectId == subjectId).ProjectTo<NewsDto>();
        }

        private string PrepareUniqueUrlName(string baseUrlName)
        {
            if (!_context.Subjects.Any(p => p.UrlName == baseUrlName))
                return baseUrlName;
            for (int i = 2; i < SameTitleSubjectsMaxNumber; i++)
            {
                string bufName = baseUrlName + i;
                if (!_context.Pages.Any(p => p.UrlName == bufName))
                    return bufName;
            }
            throw new PropertyValidationException("subject.UrlName", "Przekroczono liczbę przedmiotów o tym samym tytule.");
        }

        public NewsDto UpdateNews(int subjectId, NewsDto newsDto)
        {
            var dbNews = _context.News.Find(newsDto.Id);
            if (dbNews == null)
                throw new NotFoundException("News with id: " + newsDto.Id);
            if (subjectId != dbNews.SubjectId)
                throw new PropertyValidationException("subjectId", "");
            dbNews.Header = newsDto.Header;
            dbNews.Content = newsDto.Content;
            _context.SetModified(dbNews);
            _context.SaveChanges();
            return Mapper.Map<NewsDto>(dbNews);
        }

        public IEnumerable<User> GetStudents(int subjectId, int limit, int offset)
        {
            return _context.Subjects
                           .Where(s => s.Id == subjectId)
                           .Include(s => s.Students)
                           .SelectMany(s => s.Students)
                           .OrderBy(s => s.LastName)
                           .ThenBy(s => s.FirstName)
                           .Skip(offset)
                           .Take(limit).ToList();
        }

        public void SignUpForSubject(int subjectId, string studentId)
        {
            if (_context.SignUpRequests.Any(r => r.StudentId == studentId && r.SubjectId == subjectId))
                throw new InvalidOperationException("Request already exists.");

            var request = new SignUpRequest(subjectId, studentId);
            _context.SignUpRequests.Add(request);

            _context.SaveChanges();
        }

        public void ResignFromSubject(int subjectId, string studentId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if(subject==null)
                throw new NotFoundException("Subject with id: "+subjectId);

            var user = _context.Users.Find(studentId);
            if(user==null)
                throw new NotFoundException("User with id: "+studentId);

            subject.Students.Remove(user);

            _context.SaveChanges();
        }


        public void ApproveRequest(int requestId)
        {
            var request = _context.SignUpRequests.Find(requestId);
            if (request == null)
                throw new NotFoundException("Request with id: " + requestId);
            if(request.Status==RequestStatus.Approved)
                throw new InvalidOperationException("Cannot approve approved status");
            request.Approve();

            _context.SaveChanges();
        }

        public void RefuseRequest(int requestId)
        {
            var request = _context.SignUpRequests.Find(requestId);
            if (request == null)
                throw new NotFoundException("Request with id: " + requestId);

            if(request.Status==RequestStatus.Submitted)
                request.Refuse();
            else throw new InvalidOperationException("Cannot refuse refused or approved status");

            _context.SaveChanges();
        }

        public IEnumerable<SignUpRequest> GetSubmittedRequests(int subjectId, int limit, int offset)
        {
            return
                _context.SignUpRequests.Where(r => r.SubjectId == subjectId && r.Status == RequestStatus.Submitted)
                    .OrderByDescending(r => r.CreateTime)
                    .Skip(offset)
                    .Take(limit);
        }

        public IEnumerable<SignUpRequest> GetRefusedRequests(int subjectId, int limit, int offset)
        {
            return _context.SignUpRequests.Where(r => r.SubjectId == subjectId && r.Status == RequestStatus.Refused)
                .OrderByDescending(r => r.CreateTime)
                .Skip(offset)
                .Take(limit);
        }

        public IEnumerable<SignUpRequest> GetAllRequests(int subjectId, int limit, int offset)
        {
            return _context.SignUpRequests.Where(r => r.SubjectId == subjectId);
        }
    }
}
