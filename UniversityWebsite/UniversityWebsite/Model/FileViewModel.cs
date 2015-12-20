using System;

namespace UniversityWebsite.Model
{
    public class FileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Version { get; set; }
        //public long Size { get; set; }
    }
}