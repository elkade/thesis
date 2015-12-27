using System.Collections.Generic;

namespace UniversityWebsite.Api.Model
{
    public class PaginationVm<T>
    {
        public PaginationVm(IEnumerable<T> elements, int number, int limit, int offset)
        {
            Elements = elements;
            Number = number;
            Limit = limit;
            Offset = offset;
        }
        public IEnumerable<T> Elements { get; set; }
        public int Number { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}