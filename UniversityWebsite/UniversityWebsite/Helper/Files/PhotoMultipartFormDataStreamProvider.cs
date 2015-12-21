using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace UniversityWebsite.Helper.Files
{
    public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly string _fileId;

        public string Name { get; private set; }

        private readonly string[] _imageExts= {".bmp", ".jpg", ".png", ".gif", ".tiff", ".jpeg"};

        public PhotoMultipartFormDataStreamProvider(string path, string fileId)
            : base(path)
        {
            _fileId = fileId;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            Name = headers.ContentDisposition.FileName.Trim(new []{'"'});
            if (Name == null)
                throw new ArgumentException("Name cannot be null");
            var ext = Path.GetExtension(Name);
            if(!_imageExts.Contains(ext))
                throw new ArgumentException("Wrong content type.");
            return _fileId;
        }
    }
}