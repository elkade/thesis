using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UniversityWebsite.Model;

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
}
