using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace UniversityWebsite.Helper.Files
{
    public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly string _fileId;

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        private readonly string[] _imageExts= {".bmp", ".jpg", ".png", ".gif", ".tiff", ".jpeg"};

        public PhotoMultipartFormDataStreamProvider(string path, string fileId)
            : base(path)
        {
            _fileId = fileId;
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            _name = headers.ContentDisposition.FileName.Trim(new []{'"'});
            if (_name == null)
                throw new ArgumentException("Name cannot be null");
            var ext = Path.GetExtension(_name);
            if(!_imageExts.Contains(ext))
                throw new ArgumentException("Wrong content type.");
            return _fileId;
        }
    }
}