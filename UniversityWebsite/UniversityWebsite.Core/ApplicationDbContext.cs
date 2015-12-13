using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Domain
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DomainTestContext", throwIfV1Schema: false)
        {
        }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

    }

    
}