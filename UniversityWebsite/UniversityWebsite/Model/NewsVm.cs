using System;

namespace UniversityWebsite.Model
{
    public class NewsVm
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
    }
}