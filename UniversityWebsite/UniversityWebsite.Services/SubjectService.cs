using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Enums;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis realizujący logikę biznesową dotyczącą przedmiotów systemu.
    /// </summary>
    public interface ISubjectService
    {
        /// <summary>
        /// Zwraca zbiór przedmiotów przypisanych do danego semestru.
        /// </summary>
        /// <param name="number">Numer semestru</param>
        /// <param name="limit">Maksymalna liczba zwróconych przedmiotów</param>
        /// <param name="offset">Numer porządkowy pierwszego zwróconego przedmiotu</param>
        /// <returns>Zbiór przedmiotów</returns>
        IEnumerable<Subject> GetSubjectsBySemester(int number, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę przedmiotów w danym semestrze
        /// </summary>
        /// <param name="number">Numer semestru</param>
        /// <returns>Liczba naturalna</returns>
        int GetSubjectsNumberBySemestser(int number);
        /// <summary>
        /// Wyszukuje przedmiot o podanej nazwie.
        /// </summary>
        /// <param name="name">Nazwa przedmiotu widoczna w linku</param>
        /// <returns>Szukany przedmiot</returns>
        Subject GetSubject(string name);
        /// <summary>
        /// Zwraca zbiór wszystkich przedmiotów.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwróconych przedmiotów</param>
        /// <param name="offset">Numer porządkowy pierwszego zwróconego przedmiotu</param>
        /// <param name="userId">Id użytkonika, jeśli jest podane list przedmiotów bedzie zwrócona dla których podany użytkownik jest nauczycielem</param>
        /// <returns>Zbiór przedmitów</returns>
        IEnumerable<SubjectDto> GetSubjects(int limit, int offset, string userId = null);
        /// <summary>
        /// Zwraca liczbe wszystkich przedmiotów w systemie
        /// </summary>
        /// <returns>Liczba naturalna</returns>
        int GetSubjectsNumber();



        /// <summary>
        /// Dodaje nowy przedmiot do systemu.
        /// </summary>
        /// <param name="subject">Dane przedmiotu</param>
        /// <param name="authorId">Id użytkownika tworzącego przedmiot</param>
        /// <returns>Dane dodanego przedmiotu.</returns>
        SubjectDto AddSubject(SubjectDto subject, string authorId);
        /// <summary>
        /// Aktualizuje dane przedmiotu istniejącego w systemie.
        /// </summary>
        /// <param name="subject">Dane przedmiotu</param>
        /// <param name="authorId">Id użytkownika aktualizującego przedmiot</param>
        /// <returns>Dane przedmiotu po edycji.</returns>
        SubjectDto UpdateSubject(SubjectDto subject, string authorId);
        /// <summary>
        /// Dodaje wpis w sekcji aktualności przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="newsDto">Dane wpisu</param>
        /// <param name="authorId">Id użytkownika dokonującego edycji</param>
        /// <returns>Dane dodanego wpisu.</returns>
        NewsDto AddNews(int subjectId, NewsDto newsDto, string authorId);
        /// <summary>
        /// Zwraca wpis w sekcji aktualności.
        /// </summary>
        /// <param name="subjectId">Id wpisu.</param>
        /// <returns>Dana dodanego wpisu.</returns>
        IEnumerable<NewsDto> GetNews(int subjectId);
        /// <summary>
        /// Usuwa wpis w sekcji aktualności przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="newsId">Id wpisu</param>
        void DeleteNews(int subjectId, int newsId);
        /// <summary>
        /// Aktualizuje wpis w aktualnościach przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="newsDto">Dane wpisu</param>
        /// <returns>Dane wpisu po edycji.</returns>
        NewsDto UpdateNews(int subjectId, NewsDto newsDto);
        /// <summary>
        /// Zwraca liczbę wpisów w sekcji aktualności dla danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu.</param>
        /// <returns>Liczba naturalna.</returns>
        int GetNewsNumber(int subjectId);
        /// <summary>
        /// Zwraca zbiór studentów zapisanych na dany przedmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwróconych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego zwróconego obiektu</param>
        /// <returns>Zbiór obiektów reprezentujących studentów.</returns>
        IEnumerable<User> GetStudents(int subjectId, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę studentów zapisanych na przedmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu.</param>
        /// <returns>Liczba naturalna.</returns>
        int GetStudentsNumber(int subjectId);
        /// <summary>
        /// Zwraca zbiór nauczycieli administrujących danym przedmiotem.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu.</param>
        /// <returns>Zbiór obiektów reprezentujących nauczycieli.</returns>
        IEnumerable<User> GetTeachers(int subjectId);
        /// <summary>
        /// Dodaje nauczycieli do zbioru administratorów przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="teacherIds">Zbiór id nauczycieli</param>
        void AddTeachers(int subjectId, IEnumerable<string> teacherIds);
        /// <summary>
        /// Usuwa nauczycieli ze zbioru administratorów przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="teacherIds">Zbiór id nauczycieli</param>
        void DeleteTeachers(int subjectId, IEnumerable<string> teacherIds);
        /// <summary>
        /// Usuwa przedmiot z systemu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        void DeleteSubject(int subjectId);
        /// <summary>
        /// Zwraca identyfikator dostępnej akcji zmiany statusu wniosku o rejestrację na przedmiot przez studenta.
        /// </summary>
        /// <param name="studentId">Id studenta</param>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Id akcji</returns>
        SignUpAction GetAvailableAction(string studentId, int subjectId);
        /// <summary>
        /// Dodaje do systemu wniosek studenta o zapisanie go na przedmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="userId">Id studenta</param>
        void SignUpForSubject(int subjectId, string userId);
        /// <summary>
        /// Usuwa zo systemu wniosek studenta o zapisanie go na przedmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="studentId">Id studenta</param>
        void ResignFromSubject(int subjectId, string studentId);
        /// <summary>
        /// Zmienia status wniosków na "Pozytywnie rozpatrzony" jeżeli dotyczą one przedmiotów zarządzanych przez danego nauczyciela.
        /// </summary>
        /// <param name="requestIds">Zbiór id wniosków</param>
        /// <param name="teacherId">Id nauczyciela</param>
        void ApproveRequests(IEnumerable<int> requestIds, string teacherId);
        /// <summary>
        /// Zmienia status wniosków na "Odrzucony" jeżeli dotyczą one przedmiotów zarządzanych przez danego nauczyciela.
        /// </summary>
        /// <param name="requestIds">Zbiór id wniosków</param>
        /// <param name="teacherId">Id nauczyciela</param>
        void RefuseRequests(IEnumerable<int> requestIds, string teacherId);
        /// <summary>
        /// Zwraca zbiór wniosków o zapisanie na przedmiot dotyczących przedmiotów zarządzanych przez danego nauczyciela.
        /// </summary>
        /// <param name="userId">Id nauczyciela</param>
        /// <param name="limit">Maksymalna liczba zwróconych wniosków</param>
        /// <param name="offset">Numer porządkowy pierwszego zwróconego wniosku</param>
        /// <returns>Zbiór wniosków</returns>
        IEnumerable<SignUpRequest> GetRequestsByTeacher(string userId, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę wniosków o zapisanie na przedmiot dotyczących przedmiotów zarządzanych przez danego nauczyciela.
        /// </summary>
        /// <param name="teacherId">Id nauczyciela</param>
        /// <returns>Liczb naturalna.</returns>
        int GetRequestsNumberByTeacher(string teacherId);
        /// <summary>
        /// Zwraca zbiór wniosków o zapisania się na przedmiot dotyczących danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwróconych wniosków</param>
        /// <param name="offset">Numer porządkowy pierwszego zwróconego wniosku</param>
        /// <returns>Zbiór wniosków.</returns>
        IEnumerable<SignUpRequest> GetRequestsBySubject(int subjectId, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę wniosków o zapisania się na przedmiot dotyczących danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Liczba naturalna</returns>
        int GetRequestsNumberBySubject(int subjectId);

        /// <summary>
        /// Usuwa listę podanych studentów z przedmiotu
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="studentsIds"></param>
        void RemoveFromSubject(int subjectId, string[] studentsIds);
    }
    /// <summary>
    /// Implementacja serwisu realizującego logikę biznesową dotyczącą przedmiotów systemu.
    /// </summary>
    public class SubjectService : ISubjectService
    {
        private readonly IDomainContext _context;
        private readonly ApplicationUserManager _userManager;
        private const int SameTitleSubjectsMaxNumber = 100;

        /// <summary>
        /// Tworzy nową instancję 
        /// </summary>
        /// <param name="context">Kontekst domeny systemu.</param>
        /// <param name="userManager">Manaer odpowiedzialny za bezpośrednie operacje na użytkowniku systemu.</param>
        public SubjectService(IDomainContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public SignUpAction GetAvailableAction(string studentId, int subjectId)
        {
            var request =
                _context.SignUpRequests.SingleOrDefault(r => r.StudentId == studentId && r.SubjectId == subjectId);
            if (request == null)
                return SignUpAction.NotSubmitted;
            if (request.Status == RequestStatus.Submitted)
                return SignUpAction.Submitted;
            if (request.Status == RequestStatus.Approved)
                return SignUpAction.Approved;
            if (request.Status == RequestStatus.Refused)
                return SignUpAction.Refused;
            return SignUpAction.None;
        }

        public IEnumerable<Subject> GetSubjectsBySemester(int number, int limit, int offset)
        {
            return _context.Subjects.Where(s => s.Semester == number).OrderBy(s => s.Name).Skip(offset).Take(limit);
        }

        public int GetSubjectsNumberBySemestser(int number)
        {
            return _context.Subjects.Count(s => s.Semester == number);
        }

        public Subject GetSubject(string name)
        {
            var subject = _context.Subjects.SingleOrDefault(s => s.UrlName == name);
            return subject;
        }

        public IEnumerable<SubjectDto> GetSubjects(int limit, int offset, string userId = null)
        {
            var subjectQuery = _context.Subjects.AsQueryable();
            if (userId != null)
            {
                subjectQuery = subjectQuery.Where(s => s.Teachers.Any(t => t.Teacher.Id == userId));
            } 
            return subjectQuery
                    .OrderBy(s => s.Semester)
                    .ThenBy(s => s.Name)
                    .Skip(offset)
                    .Take(limit)
                    .ProjectTo<SubjectDto>();
        }

        public int GetSubjectsNumber()
        {
            return _context.Subjects.Count();
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
            return _context.InTransaction(() =>
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
            });
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
            _context.InTransaction(() =>
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
            });
        }

        public IEnumerable<NewsDto> GetNews(int subjectId)
        {
            return _context.News.Where(n => n.SubjectId == subjectId).ProjectTo<NewsDto>();
        }

        public int GetNewsNumber(int subjectId)
        {
            return _context.News.Count(n => n.SubjectId == subjectId);
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
            throw new PropertyValidationException("subject.UrlName",
                "Przekroczono liczbę przedmiotów o tym samym tytule.");

        }

        public NewsDto UpdateNews(int subjectId, NewsDto newsDto)
        {
            return _context.InTransaction(() =>
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
            });
        }

        public IEnumerable<User> GetTeachers(int subjectId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if(subject==null)
                throw new NotFoundException("Subject with id: "+subjectId);
            return subject.Teachers.Select(t=>t.Teacher);
        }

        public void AddTeachers(int subjectId, IEnumerable<string> teacherIds)
        {
            _context.InTransaction(() =>
            {
                var subject = _context.Subjects.Find(subjectId);
                if (subject == null)
                    throw new NotFoundException("Subject with id: " + subjectId);

                foreach (var teacherId in teacherIds)
                {
                    if (!_userManager.IsInRole(teacherId, "Teacher"))
                        throw new PropertyValidationException("teacherIds", "User with id: " + teacherId + " is not a teacher");
                    if (subject.Teachers.All(t => t.TeacherId != teacherId))
                        subject.Teachers.Add(new TeacherSubject { TeacherId = teacherId, SubjectId = subjectId });
                    else throw new PropertyValidationException("teacherIds", "Teacher with id: " + teacherId + " is already assigned to subject "+subjectId);
                }
                _context.SaveChanges();
            });
        }

        public void DeleteTeachers(int subjectId, IEnumerable<string> teacherIds)
        {
            _context.InTransaction(() =>
            {
                var subject = _context.Subjects.Find(subjectId);
                if (subject == null)
                    throw new NotFoundException("Subject with id: " + subjectId);

                foreach (var teacherId in teacherIds)
                {
                    if (!_userManager.IsInRole(teacherId, "Teacher"))
                        throw new PropertyValidationException("teacherIds", "User with id: " + teacherId + " is not a teacher");
                    var teacher = subject.Teachers.SingleOrDefault(t => t.TeacherId == teacherId);
                    if(teacher==null)
                        throw new PropertyValidationException("teacherIds", "Teacher with id: " + teacherId + " is not assigned to subject " + subjectId);
                    _context.SetDeleted(teacher);
                }
                _context.SaveChanges();
            });
        }

        public IEnumerable<User> GetStudents(int subjectId, int limit, int offset)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject with id: " + subjectId);
            return
                subject.SignUpRequests.Where(r => r.Status == RequestStatus.Approved)
                    .OrderBy(r => r.CreateTime)
                    .Skip(offset)
                    .Take(limit)
                    .Select(r => r.Student);
        }

        public int GetStudentsNumber(int subjectId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject with id: " + subjectId);
            return
                subject.SignUpRequests.Count(r => r.Status == RequestStatus.Approved);
        }

        public void SignUpForSubject(int subjectId, string studentId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject with Id: " + subjectId);
            if (_context.SignUpRequests.Any(r => r.StudentId == studentId && r.SubjectId == subjectId))
                throw new InvalidOperationException("Request already exists.");

            var request = new SignUpRequest(subjectId, studentId);
            _context.SignUpRequests.Add(request);

            _context.SaveChanges();
        }

        public void ResignFromSubject(int subjectId, string studentId)
        {
            var request = _context.SignUpRequests.SingleOrDefault(r => r.StudentId == studentId && r.SubjectId == subjectId);
            if (request == null) throw new NotFoundException("SignUpRequest with subjectId: " + subjectId + " and studentId: " + studentId);

            if (request.Status == RequestStatus.Approved || request.Status == RequestStatus.Submitted)
                _context.SetDeleted(request);
            else throw new InvalidOperationException("Cannot delete status " + request.Status);
            _context.SaveChanges();
        }

        public void RemoveFromSubject(int subjectId, string[] studentsIds)
        {
            var requests =
                _context.SignUpRequests.Where(r => r.SubjectId == subjectId && studentsIds.Contains(r.StudentId));
            foreach (var signUpRequest in requests)
            {
                _context.SetDeleted(signUpRequest);
            }
            _context.SaveChanges();
        }

        public void ApproveRequests(IEnumerable<int> requestIds, string teacherId)
        {
            _context.InTransaction(() =>
            {
                foreach (var requestId in requestIds)
                {
                    var request = _context.SignUpRequests.Find(requestId);
                    if (request == null)
                        throw new NotFoundException("Request with id: " + requestId);
                    if (!request.Subject.HasTeacher(teacherId))
                        throw new UnauthorizedAccessException("Teacher with id: " + teacherId + " does not have access to subject with id: " + request.Subject.Id);
                    if (request.Status == RequestStatus.Approved)
                        throw new InvalidOperationException("Cannot approve approved status");

                    request.Approve();
                }
                _context.SaveChanges();
            });
        }

        public void RefuseRequests(IEnumerable<int> requestIds, string teacherId)
        {
            _context.InTransaction(() =>
            {
                foreach (var requestId in requestIds)
                {
                    var request = _context.SignUpRequests.Find(requestId);
                    if (request == null)
                        throw new NotFoundException("Request with id: " + requestId);
                    if (!request.Subject.HasTeacher(teacherId))
                        throw new UnauthorizedAccessException("Teacher with id: " + teacherId + " does not have access to subject with id: " + request.Subject.Id);
                    if (request.Status == RequestStatus.Submitted)
                        request.Refuse();
                    else throw new InvalidOperationException("Cannot refuse refused or approved status");

                    _context.SaveChanges();
                }
            });
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

        public IEnumerable<SignUpRequest> GetRequestsBySubject(int subjectId, int limit, int offset)
        {
            return
                _context.SignUpRequests.Where(r => r.SubjectId == subjectId && (r.Status == RequestStatus.Submitted || r.Status == RequestStatus.Refused))
                    .OrderBy(r => r.Status)
                    .ThenBy(r => r.CreateTime)
                    .Skip(offset)
                    .Take(limit);
        }

        public int GetRequestsNumberBySubject(int subjectId)
        {
            return _context.SignUpRequests.Count(r => r.Subject.Id == subjectId);
        }

        public IEnumerable<SignUpRequest> GetRequestsByTeacher(string teacherId, int limit, int offset)
        {
            var requests =
                _context.SignUpRequests.Where(r => r.Subject.Teachers.Any(t => t.TeacherId == teacherId) ).ToList();
            requests = requests.Where(r=> (r.Status == RequestStatus.Submitted || r.Status == RequestStatus.Refused))
                    .OrderBy(r=>r.Status)
                    .ThenBy(r => r.CreateTime)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

            return requests;
        }

        public int GetRequestsNumberByTeacher(string teacherId)
        {
            return _context.SignUpRequests.Count(r => r.Subject.Teachers.Any(t => t.TeacherId == teacherId));
        }
    }
}
