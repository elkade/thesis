using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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