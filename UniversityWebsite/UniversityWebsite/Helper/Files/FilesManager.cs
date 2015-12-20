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
    public class FilesManager : IFilesManager
    {

        private string WorkingFolder { get; set; }

        private readonly IDomainContext _context;

        public FilesManager()
        {
            WorkingFolder = HttpRuntime.AppDomainAppPath + @"\Files\{0}";
            //CheckTargetDirectory();
            _context = new DomainContext();
        }

        public FilesManager(string workingFolder)
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

        public async Task<IEnumerable<FileViewModel>> GetBySubject(int subjectId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null) throw new NotFoundException("Subject with id: " + subjectId);

            var files = subject.Files.ToList();


            var fileFolder = new DirectoryInfo(string.Format(WorkingFolder, ""));

            IEnumerable<FileViewModel> result = Enumerable.Empty<FileViewModel>();


            await Task.Factory.StartNew(() =>
            {
                result = fileFolder.EnumerateFiles().Join(files, fi => fi.Name, meta => meta.Id.ToString(), (fi, meta) => new FileViewModel
                {
                    Id = meta.Id,
                    Name = fi.Name,
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

        public async Task<FileViewModel> Add(HttpRequestMessage request, int? subjectId, string userId)
        {

            Guid guid = Guid.NewGuid();

            var provider = new FileMultipartFormDataStreamProvider(string.Format(WorkingFolder,""), guid.ToString());

            await request.Content.ReadAsMultipartAsync(provider);

            if (provider.Name == null)
            {
                await Task.Factory.StartNew(() => File.Delete(string.Format(WorkingFolder, guid)));
                throw new ArgumentException("Name cannot be null");
            }

            var dbFile = new Domain.Model.File
            {
                FileName = provider.Name,
                Id = guid.ToString(),
                SubjectId = subjectId ?? 0,
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

        public async Task<FileViewModel> Update(HttpRequestMessage request, int? subjectId, string userId, string fileId)
        {

            var oldFile = _context.Files.Find(fileId);

            if(oldFile==null) throw new NotFoundException("File with id: "+fileId);

            await Task.Factory.StartNew(() => File.Delete(string.Format(WorkingFolder, oldFile.Id)));

            var provider = new FileMultipartFormDataStreamProvider(string.Format(WorkingFolder, ""), oldFile.Id);

            await request.Content.ReadAsMultipartAsync(provider);

            oldFile.UpdateDate = DateTime.Now;
            oldFile.Version++;
            oldFile.UserId = userId;

            _context.Entry(oldFile).State = EntityState.Modified;

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