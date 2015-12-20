using System.Net.Http;

namespace UniversityWebsite.Helper.Files
{
    public class FileMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly string _fileId;

        private string _name;

        public string Name
        {
            get { return _name; }
        }
        

        public FileMultipartFormDataStreamProvider(string path, string fileId) : base(path)
        {
            _fileId = fileId;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            _name = headers.ContentDisposition.FileName.Trim(new []{'"'});
            return _fileId;
        }
    }
}