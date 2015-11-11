﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Core.Migrations;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
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
        IDbSet<Language> Languages { get; set; }
        IDbSet<Phrase> Phrases { get; set; }
        IDbSet<File> Files { get; set; }
        IDbSet<Teacher> Teachers { get; set; }
        IDbSet<Student> Students { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
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

        public IDbSet<Teacher> Teachers { get; set; }
        public IDbSet<Student> Students { get; set; }
        public virtual IDbSet<Semester> Semester { get; set; }
        public virtual IDbSet<Page> Pages { get; set; }
        public virtual IDbSet<Subject> Subjects { get; set; }
        public virtual IDbSet<NavigationMenu> Menus { get; set; }
        public virtual IDbSet<Language> Languages { get; set; }
        public virtual IDbSet<Phrase> Phrases { get; set; }
        public virtual IDbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>();
        }

        public static DomainContext Create()
        {
            return new DomainContext();
        }
    }
}