using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityWebsite.Services.Model
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
    }
}
