using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Core.Migrations;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Core
{
    /// <summary>
    /// Kontekst domeny aplikacji.
    /// </summary>
    public interface IDomainContext
    {
        /// <summary>
        /// Zbiór stron systemu.
        /// </summary>
        IDbSet<Page> Pages { get; set; }
        /// <summary>
        /// Zbiór przedmiotów systemu.
        /// </summary>
        IDbSet<Subject> Subjects { get; set; }
        /// <summary>
        /// Zbiór menu systemu.
        /// </summary>
        IDbSet<Menu> Menus { get; set; }
        /// <summary>
        /// Zbiór elementów menu systemu.
        /// </summary>
        IDbSet<MenuItem> MenuItems { get; set; }
        /// <summary>
        /// Zbiór użytkowników systemu.
        /// </summary>
        IDbSet<User> Users { get; set; }
        /// <summary>
        /// Zbiór ról systemu.
        /// </summary>
        IDbSet<IdentityRole> Roles { get; set; }
        /// <summary>
        /// Zbiór języków systemu.
        /// </summary>
        IDbSet<Language> Languages { get; set; }
        /// <summary>
        /// Zbiór tłumaczeń fraz systemu.
        /// </summary>
        IDbSet<Phrase> Phrases { get; set; }
        /// <summary>
        /// Zbiór danych o plikach systemu.
        /// </summary>
        IDbSet<File> Files { get; set; }
        /// <summary>
        /// Zbiór wniosków o zapisanie za przedmiot systemu.
        /// </summary>
        IDbSet<SignUpRequest> SignUpRequests { get; set; }
        /// <summary>
        /// Zbiór grup stron systemu.
        /// </summary>
        IDbSet<PageGroup> PageGroups { get; set; }
        /// <summary>
        /// Zbiór wpisów w aktualnościach stron systemu.
        /// </summary>
        IDbSet<News> News { get; set; }
        /// <summary>
        /// Zapisuje zmiany w bazie danych.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// Oznacza encję jako zmodyfikowaną.
        /// </summary>
        /// <param name="entity">Zmodyfikowana encja</param>
        void SetModified(object entity);
        /// <summary>
        /// Oznacza encję jako usuniętą.
        /// </summary>
        /// <param name="entity">Usunięta encja</param>
        void SetDeleted(object entity);
        /// <summary>
        /// Wykonuje daną delegację w transakcji bazodanowej.
        /// </summary>
        /// <typeparam name="T">Typ zwracanego przez delegację obiektu</typeparam>
        /// <param name="func">Delegacja</param>
        /// <returns>Obiekt typu T</returns>
        T InTransaction<T>(Func<T> func);
        /// <summary>
        /// Wykonuje daną delegację w transakcji bazodanowej.
        /// </summary>
        void InTransaction(Action action);
        /// <summary>
        /// Oznacza właściwość encji jako zmodyfikowaną.
        /// </summary>
        /// <typeparam name="TEntity">Typ encji</typeparam>
        /// <typeparam name="TProperty">Typ właściwości</typeparam>
        /// <param name="entity">Encja o zmodyfikowanej właściwości</param>
        /// <param name="property">Wyrażenie zwracające wartość właściwości z encji</param>
        void SetPropertyModified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : class;
    }

    /// <summary>
    /// Implementuje domeny aplikacji.
    /// </summary>
    public class DomainContext : IdentityDbContext<User>, IDomainContext
    {
        static DomainContext()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DomainContext, Configuration>("DomainContext"));
            Database.SetInitializer(new DomainContextInitializer());
        }

        /// <summary>
        /// Tworzy nową instancję domeny aplikacji.
        /// </summary>
        public DomainContext()
            : base("DomainContext", throwIfV1Schema: false)
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
        /// <summary>
        /// Tworzy nową instancję kontekstu domeny.
        /// </summary>
        /// <returns></returns>
        public static DomainContext Create()
        {
            return new DomainContext();
        }
    }
}