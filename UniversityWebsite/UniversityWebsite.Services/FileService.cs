using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UniversityWebsite.Core;
using UniversityWebsite.Helper.Files;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.FileProviders;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis odpowiadający za zarządzanie plikami zapisanymi w systemie.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Zwraca zbiór informacji o plikach należących do danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwracanych rekordów</param>
        /// <param name="offset">Numer porządkowy pierwszego zwracanego rekordu</param>
        /// <returns>Zbiór rekordów</returns>
        Task<IEnumerable<FileDto>> GetBySubject(int subjectId, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę plików przypisanych do danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Liczba naturalna</returns>
        int GetFilesNumberBySubject(int subjectId);
        /// <summary>
        /// Usuwa z bazy danych informację o pliku o danym id oraz usuwa ten plik z serwera.
        /// </summary>
        /// <param name="fileId">Id pliku</param>
        /// <returns></returns>
        Task Delete(string fileId);
        /// <summary>
        /// Dodaje do bazy danych informację o pliku oraz umieszcza plik na serwerze.
        /// </summary>
        /// <param name="request">Żądanie klienta wraz ze strumieiem dodawanego pliku</param>
        /// <param name="subjectId">Id przedmiotu, do którego ma zostać dodany plik</param>
        /// <param name="userId">Id użytkownika dodającego plik</param>
        /// <returns>Metadane dodanego pliku</returns>
        Task<FileDto> Add(HttpRequestMessage request, int subjectId, string userId);
        /// <summary>
        /// Aktualizuje plik na serwere oraz informację o nim zapisaną w bazie danych.
        /// </summary>
        /// <param name="request">Żądanie klienta wraz ze strumieiem pliku</param>
        /// <param name="userId">Id użytkownika aktualizującego plik</param>
        /// <param name="fileId">Id aktualizowanego pliku</param>
        /// <returns></returns>
        Task<FileDto> Update(HttpRequestMessage request, string userId, string fileId);
        /// <summary>
        /// Zwraca link pliku o danym id, jeżeli dany użytkownik ma do niego dostęp.
        /// </summary>
        /// <param name="fileId">Id pliku</param>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Link do pliku lub wyjątek UnauthorizedAccessException, jeżeli użytkownik nie ma dostępu do pliku.</returns>
        FileBasicInfo GetPath(string fileId, string userId);
        /// <summary>
        /// Zwraca zbiór informacji o plikach.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwróconych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu, który ma zostać zwrócony</param>
        /// <returns>Zbiór obiektów reprezentujących dane plików.</returns>
        Task<IEnumerable<FileDto>> GetGallery(int limit, int offset);
        /// <summary>
        /// Dodaje plik graficzny.
        /// </summary>
        /// <param name="request">Żądanie HTTP zawierające plik do zapisania na serwerze.</param>
        /// <param name="userId">Id użytkownika dodającego plik.</param>
        /// <returns>Metadane dodanego pliku.</returns>
        Task<FileDto> AddToGallery(HttpRequestMessage request, string userId);
        /// <summary>
        /// Aktualizuje plik graficzny.
        /// </summary>
        /// <param name="request">Żądanie HTTP zawierające plik do zapisania na serwerze.</param>
        /// <param name="userId">Id użytkownika aktualizującego plik.</param>
        /// <param name="fileId">In aktualizowanego pliku.</param>
        /// <returns>Nowe metadane zaktualizowanego pliku.</returns>
        Task<FileDto> UpdateInGallery(HttpRequestMessage request, string userId, string fileId);
        /// <summary>
        /// Zwraca liczbę Plików graficznych w galerii systemu.
        /// </summary>
        /// <returns>Liczba naturalna.</returns>
        int GetGalleryImagesNumber();
    }

    /// <summary>
    /// Implementuje serwis odpowiedzialny za zarządzanie plikami systemu. 
    /// </summary>
    public class FileService : IFileService
    {
        private readonly string _workingFolder;

        private string FilePath(string fileName)
        {
            return _workingFolder + "\\" + fileName;
        }

        private readonly IDomainContext _context;

        /// <summary>
        /// Tworzy nową instncję serwisu.
        /// </summary>
        /// <param name="context">Kontekst domeny systemu.</param>
        /// <param name="workingFolder">Ścieżka do folderu przechowującego pliki.</param>
        public FileService(IDomainContext context, string workingFolder)
        {
            _context = context;
            _workingFolder = workingFolder;

            bool exists = Directory.Exists(_workingFolder);

            if (!exists)
                Directory.CreateDirectory(_workingFolder);
        }

        public FileBasicInfo GetPath(string fileId, string userId)
        {
            var file = _context.Files.Find(fileId);
            if (file == null) throw new NotFoundException("File with fileId: " + fileId);

            //if (file.Subject != null && !file.Subject.HasStudent(userId))
            //    throw new UnauthorizedAccessException("FileId: " + fileId + " userId: " + userId);
            return new FileBasicInfo { Path = FilePath(fileId), Name = file.FileName };
        }

        public async Task<IEnumerable<FileDto>> GetGallery(int limit, int offset)
        {
            var files =
                _context.Files.Where(fi => fi.Subject == null)
                    .OrderByDescending(fi => fi.UpdateDate)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

            var fileFolder = new DirectoryInfo(_workingFolder);

            IEnumerable<FileDto> result = Enumerable.Empty<FileDto>();

            await Task.Factory.StartNew(() =>
            {
                result = fileFolder.EnumerateFiles().Join(files, fi => fi.Name, meta => meta.Id.ToString(), (fi, meta) => new FileDto
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

        public async Task<IEnumerable<FileDto>> GetBySubject(int subjectId, int limit, int offset)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null) throw new NotFoundException("Subject with fileId: " + subjectId);

            var files =
                subject.Files
                    .OrderByDescending(fi => fi.UpdateDate)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();


            var fileFolder = new DirectoryInfo(_workingFolder);

            IEnumerable<FileDto> result = Enumerable.Empty<FileDto>();


            await Task.Factory.StartNew(() =>
            {
                result = fileFolder.EnumerateFiles().Join(files, fi => fi.Name, meta => meta.Id.ToString(), (fi, meta) => new FileDto
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
            if (file == null)
                throw new NotFoundException("File with fileId: " + fileId);
            await Task.Factory.StartNew(() => File.Delete(FilePath(fileId)));
            _context.Files.Remove(file);
            _context.SaveChanges();
        }

        public async Task<FileDto> Add(HttpRequestMessage request, int subjectId, string userId)
        {
            return await _context.InTransaction(async () =>
            {
                Guid guid = Guid.NewGuid();
                try
                {
                    var provider = new FileMultipartFormDataStreamProvider(_workingFolder, guid.ToString());

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

                    return new FileDto
                    {
                        Id = dbFile.Id,
                        Created = dbFile.UploadDate,
                        Modified = dbFile.UpdateDate,
                        Name = dbFile.FileName,
                        Version = dbFile.Version
                    };
                }
                catch (Exception)
                {
                    DeleteFileIfExists(guid.ToString());
                    throw;
                }
            });

        }

        private void DeleteFileIfExists(string fileName)
        {
            var path = FilePath(fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        public async Task<FileDto> AddToGallery(HttpRequestMessage request, string userId)
        {
            return await _context.InTransaction(async () =>
            {
                Guid guid = Guid.NewGuid();
                try
                {
                    var provider = new PhotoMultipartFormDataStreamProvider(_workingFolder, guid.ToString());

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

                    return new FileDto
                    {
                        Id = dbFile.Id,
                        Created = dbFile.UploadDate,
                        Modified = dbFile.UpdateDate,
                        Name = dbFile.FileName,
                        Version = dbFile.Version
                    };
                }
                catch (Exception)
                {
                    DeleteFileIfExists(guid.ToString());
                    throw;
                }
            });
        }

        public async Task<FileDto> UpdateInGallery(HttpRequestMessage request, string userId, string fileId)
        {
            return await _context.InTransaction(async () =>
            {
                var oldFile = _context.Files.Find(fileId);

                if (oldFile == null) throw new NotFoundException("File with fileId: " + fileId);

                if (oldFile.Subject != null)
                    throw new Exception("Unathorized access.");

                await Task.Factory.StartNew(() => File.Delete(FilePath(oldFile.Id)));

                var provider = new PhotoMultipartFormDataStreamProvider(_workingFolder, oldFile.Id);

                await request.Content.ReadAsMultipartAsync(provider);

                oldFile.UpdateDate = DateTime.Now;
                oldFile.Version++;
                oldFile.UserId = userId;
                oldFile.FileName = provider.Name;

                _context.SetModified(oldFile);

                _context.SaveChanges();

                return new FileDto
                {
                    Id = oldFile.Id,
                    Created = oldFile.UploadDate,
                    Modified = oldFile.UpdateDate,
                    Name = oldFile.FileName,
                    Version = oldFile.Version
                };
            });
        }

        public int GetFilesNumberBySubject(int subjectId)
        {
            return _context.Files.Count(f => f.SubjectId == subjectId);
        }

        public int GetGalleryImagesNumber()
        {
            return _context.Files.Count(f => f.SubjectId == null);
        }


        public async Task<FileDto> Update(HttpRequestMessage request, string userId, string fileId)
        {
            return await _context.InTransaction(async () =>
            {
                var oldFile = _context.Files.Find(fileId);

                if (oldFile == null) throw new NotFoundException("File with fileId: " + fileId);

                await Task.Factory.StartNew(() => File.Delete(FilePath(oldFile.Id)));

                var provider = new FileMultipartFormDataStreamProvider(_workingFolder, oldFile.Id);

                await request.Content.ReadAsMultipartAsync(provider);

                oldFile.UpdateDate = DateTime.Now;
                oldFile.Version++;
                oldFile.UserId = userId;
                oldFile.FileName = provider.Name;

                _context.SetModified(oldFile);

                _context.SaveChanges();

                return new FileDto
                {
                    Id = oldFile.Id,
                    Created = oldFile.UploadDate,
                    Modified = oldFile.UpdateDate,
                    Name = oldFile.FileName,
                    Version = oldFile.Version
                };
            });
        }
    }
}