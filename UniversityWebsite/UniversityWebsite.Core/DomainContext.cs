using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Core.Migrations;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Core
{
    public interface IDomainContext
    {
        IDbSet<Page> Pages { get; set; }
        IDbSet<Subject> Subjects { get; set; }
        IDbSet<Menu> Menus { get; set; }
        IDbSet<MenuItem> MenuItems { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<IdentityRole> Roles { get; set; }
        IDbSet<Language> Languages { get; set; }
        IDbSet<Phrase> Phrases { get; set; }
        IDbSet<File> Files { get; set; }
        IDbSet<SignUpRequest> SignUpRequests { get; set; }
        IDbSet<PageGroup> PageGroups { get; set; }
        IDbSet<News> News { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Database Database { get; }

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
        public virtual IDbSet<Menu> Menus { get; set; }
        public virtual IDbSet<MenuItem> MenuItems { get; set; }
        public virtual IDbSet<Language> Languages { get; set; }
        public virtual IDbSet<Phrase> Phrases { get; set; }
        public virtual IDbSet<File> Files { get; set; }
        public virtual IDbSet<SignUpRequest> SignUpRequests { get; set; }
        public virtual IDbSet<PageGroup> PageGroups { get; set; }
        public virtual IDbSet<News> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static DomainContext Create()
        {
            return new DomainContext();
        }
    }
}