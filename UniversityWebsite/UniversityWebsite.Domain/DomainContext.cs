using System.Data.Entity;
using System.Threading.Tasks;
using UniversityWebsite.Domain.Migrations;

namespace UniversityWebsite.Domain
{
    public interface IDomainContext
    {
        IDbSet<Page> Pages { get; set; }
        IDbSet<Subject> Subjects { get; set; }
        IDbSet<NavigationMenu> Menus { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public class DomainContext : DbContext, IDomainContext
    {
        static DomainContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DomainContext, Configuration>("DomainContext"));
        }

        public DomainContext() : base("name=DomainContext")
        {
        }

        public virtual IDbSet<Page> Pages { get; set; }
        public virtual IDbSet<Subject> Subjects { get; set; }
        public virtual IDbSet<NavigationMenu> Menus { get; set; }
    }
}