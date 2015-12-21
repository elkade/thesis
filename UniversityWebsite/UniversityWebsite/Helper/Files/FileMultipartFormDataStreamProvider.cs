using System;
using System.Net.Http;

namespace UniversityWebsite.Helper.Files
{
    public class FileMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly string _fileId;

        public string Name { get; private set; }


        public FileMultipartFormDataStreamProvider(string path, string fileId) : base(path)
        {
            _fileId = fileId;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            Name = headers.ContentDisposition.FileName.Trim(new []{'"'});
            if(Name==null)
                throw new ArgumentException("Name cannot be null");
            return _fileId;
        }
    }
}