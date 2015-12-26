using System;

namespace UniversityWebsite.Services.Model
{
    public class FileDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Version { get; set; }
        //public long Size { get; set; }
    }
}