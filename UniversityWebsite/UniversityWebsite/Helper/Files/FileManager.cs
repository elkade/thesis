using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using UniversityWebsite.Core;
using UniversityWebsite.Model;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Helper.Files
{
    public interface IFileManager
    {
        Task<IEnumerable<FileViewModel>> GetBySubject(int subjectId, int limit, int offset);
        Task Delete(string fileId);
        Task<FileViewModel> Add(HttpRequestMessage request, int subjectId, string userId);
        Task<FileViewModel> Update(HttpRequestMessage request, string userId, string fileId);
        FileGetInfo GetPath(string id, string userId);
        Task<IEnumerable<FileViewModel>> GetGallery(int limit, int offset);
        Task<FileViewModel> AddToGallery(HttpRequestMessage request, string userId);
        Task<FileViewModel> UpdateInGallery(HttpRequestMessage request, string userId, string fileId);
    }

    public class FileManager : IFileManager
    {

        private string WorkingFolder { get; set; }

        private readonly IDomainContext _context;

        public FileManager()
        {
            WorkingFolder = HttpRuntime.AppDomainAppPath + @"\Files\{0}";
            //CheckTargetDirectory();
            _context = new DomainContext();
        }

        public FileManager(string workingFolder)
        {
            this.WorkingFolder = workingFolder;
            //CheckTargetDirectory();
        }

        public FileGetInfo GetPath(string id, string userId)
        {
            var fileInfo = _context.Files.Find(id);
            if (fileInfo == null) throw new NotFoundException("File with id: " + id);

            //if(fileInfo.Subject!=null)
            //    if(!fileInfo.Subject.Students.Select(s=>s.Id).Contains(userId))
            //        throw new Exception();
            return new FileGetInfo { Path = string.Format(WorkingFolder, id), Name = fileInfo.FileName };
        }

        public async Task<IEnumerable<FileViewModel>> GetGallery(int limit, int offset)
        {
            var files =
                _context.Files.Where(fi => fi.Subject == null)
                    .OrderByDescending(fi => fi.UpdateDate)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

            var fileFolder = new DirectoryInfo(string.Format(WorkingFolder, ""));

            IEnumerable<FileViewModel> result = Enumerable.Empty<FileViewModel>();

            await Task.Factory.StartNew(() =>
            {
                result = fileFolder.EnumerateFiles().Join(files, fi => fi.Name, meta => meta.Id.ToString(), (fi, meta) => new FileViewModel
                {
                    Id = meta.Id,
                    Name = meta.FileName,
                    Created = fi.CreationTime,
                    Modified = fi.LastWriteTime,
                    Version = meta.Version
                });
            });

            return result;
        }

        public async Task<IEnumerable<FileViewModel>> GetBySubject(int subjectId, int limit, int offset)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null) throw new NotFoundException("Subject with id: " + subjectId);

            var files =
                subject.Files
                    .OrderByDescending(fi => fi.UpdateDate)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();


            var fileFolder = new DirectoryInfo(string.Format(WorkingFolder, ""));

            IEnumerable<FileViewModel> result = Enumerable.Empty<FileViewModel>();


            await Task.Factory.StartNew(() =>
            {
                result = fileFolder.EnumerateFiles().Join(files, fi => fi.Name, meta => meta.Id.ToString(), (fi, meta) => new FileViewModel
                {
                    Id = meta.Id,
                    Name = meta.FileName,
                    Created = fi.CreationTime,
                    Modified = fi.LastWriteTime,
                    Version = meta.Version
                });
            });

            return result;
        }

        public async Task Delete(string fileId)
        {
            var file = _context.Files.Find(fileId);
            if(file==null)
                throw new NotFoundException("File with id: "+fileId);
            await Task.Factory.StartNew(() => File.Delete(string.Format(WorkingFolder,fileId)));
            _context.Files.Remove(file);
            _context.SaveChanges();
        }

        public async Task<FileViewModel> Add(HttpRequestMessage request, int subjectId, string userId)
        {

            Guid guid = Guid.NewGuid();

            var provider = new FileMultipartFormDataStreamProvider(string.Format(WorkingFolder,""), guid.ToString());

            await request.Content.ReadAsMultipartAsync(provider);

            var dbFile = new Domain.Model.File
            {
                FileName = provider.Name,
                Id = guid.ToString(),
                SubjectId = subjectId,
                UpdateDate = DateTime.Now,
                UploadDate = DateTime.Now,
                UserId = userId,
            };

            _context.Files.Add(dbFile);

            _context.SaveChanges();

            return new FileViewModel
            {
                Id = dbFile.Id,
                Created = dbFile.UploadDate,
                Modified = dbFile.UpdateDate,
                Name = dbFile.FileName,
                Version = dbFile.Version
            };
        }

        public async Task<FileViewModel> AddToGallery(HttpRequestMessage request, string userId)
        {

            Guid guid = Guid.NewGuid();

            var provider = new PhotoMultipartFormDataStreamProvider(string.Format(WorkingFolder, ""), guid.ToString());

            await request.Content.ReadAsMultipartAsync(provider);

            var dbFile = new Domain.Model.File
            {
                FileName = provider.Name,
                Id = guid.ToString(),
                SubjectId = null,
                UpdateDate = DateTime.Now,
                UploadDate = DateTime.Now,
                UserId = userId,
            };

            _context.Files.Add(dbFile);

            _context.SaveChanges();

            return new FileViewModel
            {
                Id = dbFile.Id,
                Created = dbFile.UploadDate,
                Modified = dbFile.UpdateDate,
                Name = dbFile.FileName,
                Version = dbFile.Version
            };
        }

        public async Task<FileViewModel> UpdateInGallery(HttpRequestMessage request, string userId, string fileId)
        {
            var oldFile = _context.Files.Find(fileId);

            if (oldFile == null) throw new NotFoundException("File with id: " + fileId);

            if(oldFile.Subject!=null)
                throw new Exception("Unathorized access.");

            await Task.Factory.StartNew(() => File.Delete(string.Format(WorkingFolder, oldFile.Id)));

            var provider = new PhotoMultipartFormDataStreamProvider(string.Format(WorkingFolder, ""), oldFile.Id);

            await request.Content.ReadAsMultipartAsync(provider);

            oldFile.UpdateDate = DateTime.Now;
            oldFile.Version++;
            oldFile.UserId = userId;
            oldFile.FileName = provider.Name;

            _context.SetModified(oldFile);

            _context.SaveChanges();

            return new FileViewModel
            {
                Id = oldFile.Id,
                Created = oldFile.UploadDate,
                Modified = oldFile.UpdateDate,
                Name = oldFile.FileName,
                Version = oldFile.Version
            };
        }

        public async Task<FileViewModel> Update(HttpRequestMessage request, string userId, string fileId)
        {
            var oldFile = _context.Files.Find(fileId);

            if(oldFile == null) throw new NotFoundException("File with id: "+fileId);

            await Task.Factory.StartNew(() => File.Delete(string.Format(WorkingFolder, oldFile.Id)));

            var provider = new FileMultipartFormDataStreamProvider(string.Format(WorkingFolder, ""), oldFile.Id);

            await request.Content.ReadAsMultipartAsync(provider);

            oldFile.UpdateDate = DateTime.Now;
            oldFile.Version++;
            oldFile.UserId = userId;
            oldFile.FileName = provider.Name;

            _context.SetModified(oldFile);

            _context.SaveChanges();

            return new FileViewModel
            {
                Id = oldFile.Id,
                Created = oldFile.UploadDate,
                Modified = oldFile.UpdateDate,
                Name = oldFile.FileName,
                Version = oldFile.Version
            };
        }
    }
}