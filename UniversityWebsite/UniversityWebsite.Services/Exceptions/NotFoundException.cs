using System;

namespace UniversityWebsite.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
                
        }
    }
}