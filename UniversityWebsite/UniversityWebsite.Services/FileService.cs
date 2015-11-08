using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IFileService
    {
        void AddFile(File file);
        void UpdateFile(File file);
        File FindFile(string fileName);
        void DeleteFile(string fileName);
    }
    public class FileService : IFileService
    {
        private IDomainContext _domainContext;
        public FileService(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        public void AddFile(File file)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFile(File file)
        {
            throw new System.NotImplementedException();
        }

        public File FindFile(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
