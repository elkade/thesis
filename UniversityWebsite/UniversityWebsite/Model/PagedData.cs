using System.Collections.Generic;

namespace UniversityWebsite.Model
{
    public class PagedData<T> where T : class 
    {
        public IEnumerable<T> Data { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}