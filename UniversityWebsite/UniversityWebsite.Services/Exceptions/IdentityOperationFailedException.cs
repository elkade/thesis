using System;
using Microsoft.AspNet.Identity;

namespace UniversityWebsite.Services.Exceptions
{
    public class IdentityOperationFailedException : Exception
    {
        public IdentityOperationFailedException(IdentityResult identityResult)
        {
            IdentityResult = identityResult;
        }
        public IdentityResult IdentityResult { get; private set; }
    }
}