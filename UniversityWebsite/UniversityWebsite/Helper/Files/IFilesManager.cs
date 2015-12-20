using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UniversityWebsite.Model;

namespace UniversityWebsite.Helper.Files
{
    public interface IFilesManager
    {
        Task<IEnumerable<FileViewModel>> GetBySubject(int subjectId);
        Task Delete(string fileId);
        Task<FileViewModel> Add(HttpRequestMessage request, int? subjectId, string userId);
        Task<FileViewModel> Update(HttpRequestMessage request, int? subjectId, string userId, string fileId);
        FileGetInfo GetPath(string id, string userId);
    }
}
