using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Core.Migrations;
using UniversityWebsite.Domain;
using Configuration = UniversityWebsite.Core.Migrations.Configuration;

namespace UniversityWebsite.Core
{
    public interface IDomainContext
    {
        IDbSet<Page> Pages { get; set; }
        IDbSet<Subject> Subjects { get; set; }
        IDbSet<NavigationMenu> Menus { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<IdentityRole> Roles { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public class DomainContext : ApplicationDbContext, IDomainContext
    {
        static DomainContext()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DomainContext, Configuration>("DomainContext"));
            Database.SetInitializer(new DomainContextInitializer());
        }

        public DomainContext() 
        {
        }

        public virtual IDbSet<Page> Pages { get; set; }
        public virtual IDbSet<Subject> Subjects { get; set; }
        public virtual IDbSet<NavigationMenu> Menus { get; set; }

        public static DomainContext Create()
        {
            return new DomainContext();
        }
    }
}