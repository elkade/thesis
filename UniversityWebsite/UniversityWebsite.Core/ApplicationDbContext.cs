using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Core
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DomainContext", throwIfV1Schema: false)
        {
        }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

    }

    
}