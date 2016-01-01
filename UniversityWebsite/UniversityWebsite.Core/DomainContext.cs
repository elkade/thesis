using System;
using System.Data.Entity;
using System.Linq.Expressions;
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
        void SetModified(object entity);
        void SetDeleted(object entity);
        T InTransaction<T>(Func<T> func);
        void InTransaction(Action action);

        void SetPropertyModified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : class;
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

        public T InTransaction<T>(Func<T> func )
        {
            using (DbContextTransaction dbTran = Database.BeginTransaction())
            {
                try
                {
                    T result =  func();
                    dbTran.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    throw;
                }
            }
        }
        public void InTransaction(Action action)
        {
            using (DbContextTransaction dbTran = Database.BeginTransaction())
            {
                try
                {
                    action();
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    throw;
                }
            }
        }

        public void SetPropertyModified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : class
        {
            Entry(entity).Property(property).IsModified = true;
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void SetDeleted(object entity)
        {
            Entry(entity).State = EntityState.Deleted;
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