using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Core.Migrations
{
    public class InitialDataLoader
    {
        private readonly static string AdminRole = "Administrator";
        private static readonly string Admin = "su@su.su";
        private readonly static string[] RoleNames = { AdminRole, "Student", "Teacher" };
        private readonly User[] Users =
        {
            new User
            {
                UserName = Admin,
                Email = "su@su.su",
                PasswordHash = "su1234", //plain password that will be hashed
            },
        };

        private readonly IDomainContext _context;
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InitialDataLoader(DomainContext domainContext)
        {
            _context = domainContext;
            _userManager = new ApplicationUserManager(new UserStore<User>(domainContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(domainContext));
        }

        public void WithDefault()
        {
            try
            {
                WithRoles();
                WithUsers();
                WithAdmin(Admin);
                AddPagesAndMenus();
                WithSubjects();
                WithPhrases();
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string s = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    s+=string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s+=string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(s);
            }
        }

        private void WithPhrases()
        {
            _context.Phrases.Add(new Phrase { Key = "welcome", CountryCode = "pl", Value = "Witaj!" });
            _context.Phrases.Add(new Phrase { Key = "welcome", CountryCode = "en", Value = "Welcome!" });

            _context.Phrases.Add(new Phrase { Key = "manage", CountryCode = "pl", Value = "Zarządzaj" });
            _context.Phrases.Add(new Phrase { Key = "manage", CountryCode = "en", Value = "Manage" });

            _context.Phrases.Add(new Phrase { Key = "logOff", CountryCode = "pl", Value = "Wyloguj" });
            _context.Phrases.Add(new Phrase { Key = "logOff", CountryCode = "en", Value = "Sign Out" });

            _context.Phrases.Add(new Phrase { Key = "logIn", CountryCode = "pl", Value = "Zaloguj" });
            _context.Phrases.Add(new Phrase { Key = "logIn", CountryCode = "en", Value = "Sign In" });

            _context.Phrases.Add(new Phrase { Key = "email", CountryCode = "pl", Value = "Email" });
            _context.Phrases.Add(new Phrase { Key = "email", CountryCode = "en", Value = "Email" });

            _context.Phrases.Add(new Phrase { Key = "password", CountryCode = "pl", Value = "Hasło" });
            _context.Phrases.Add(new Phrase { Key = "password", CountryCode = "en", Value = "Password" });

            _context.Phrases.Add(new Phrase { Key = "teaching", CountryCode = "pl", Value = "Dydaktyka" });
            _context.Phrases.Add(new Phrase { Key = "teaching", CountryCode = "en", Value = "Teaching" });

            _context.Phrases.Add(new Phrase { Key = "home", CountryCode = "pl", Value = "Strona główna" });
            _context.Phrases.Add(new Phrase { Key = "home", CountryCode = "en", Value = "Home" });

            _context.Phrases.Add(new Phrase { Key = "semester", CountryCode = "pl", Value = "Semestr" });
            _context.Phrases.Add(new Phrase { Key = "semester", CountryCode = "en", Value = "Semester" });


            _context.Phrases.Add(new Phrase { Key = "news", CountryCode = "pl", Value = "Aktualności" });
            _context.Phrases.Add(new Phrase { Key = "news", CountryCode = "en", Value = "News" });

            _context.Phrases.Add(new Phrase { Key = "syllabus", CountryCode = "pl", Value = "Sylabus" });
            _context.Phrases.Add(new Phrase { Key = "syllabus", CountryCode = "en", Value = "Syllabus" });

            _context.Phrases.Add(new Phrase { Key = "schedule", CountryCode = "pl", Value = "Plan zajęć" });
            _context.Phrases.Add(new Phrase { Key = "schedule", CountryCode = "en", Value = "Schedule" });

            _context.Phrases.Add(new Phrase { Key = "files", CountryCode = "pl", Value = "Materiały dydaktyczne" });
            _context.Phrases.Add(new Phrase { Key = "files", CountryCode = "en", Value = "Files" });


        }

        private void AddPagesAndMenus()
        {
            var kontakt = new PageGroup();
            var badania = new PageGroup();
            var kadra = new PageGroup();

            var pl = new Language { Title = "polski", CountryCode = "pl" };
            var en = new Language { Title = "english", CountryCode = "en" };
            var pagesPl = new List<Page>
            {
                new Page
                {
                    Title = "Kontakt",
                    UrlName = "Kontakt",
                    Group = kontakt,
                    Language = pl,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                },
                new Page
                {
                    Title = "Badania",
                    UrlName = "Badania",
                    Language = pl,
                    Group = badania,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                },
                new Page
                {
                    Title = "Kadra",
                    UrlName = "Kadra",
                    Language = pl,
                    Group = kadra,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                }
            };
            foreach (var p in pagesPl)
                _context.Pages.Add(p);

            var pagesEn = new List<Page>
            {
                new Page
                {
                    Title = "Contact",
                    Language = en,
                    UrlName = "Contact",
                    Group = kontakt,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                },
                new Page
                {
                    Title = "Research",
                    Language = en,
                    UrlName = "Research",
                    Group = badania,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                },
                new Page
                {
                    Title = "Staff",
                    UrlName = "Staff",
                    Language = en,
                    Group = kadra,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                }
            };
            foreach (var p in pagesEn)
                _context.Pages.Add(p);

            var mainMenu = new MenuGroup{Id=1};
            var tileMenu = new MenuGroup{Id=2};

            var tileMenuPl = new Menu { Language = pl, Group = tileMenu };
            var tileMenuEn = new Menu { Language = en, Group = tileMenu };

            var menuPl = new Menu {Language = pl, Group = mainMenu};
            var menuEn = new Menu {Language = en, Group = mainMenu};

            menuPl.Items = pagesPl.Select((p, i) => new MenuItem { Page = p, Order = i }).ToList();
            menuEn.Items = pagesEn.Select((p, i) => new MenuItem { Page = p, Order = i }).ToList();

            _context.Menus.Add(tileMenuPl);
            _context.Menus.Add(tileMenuEn);

            _context.Menus.Add(menuPl);
            _context.Menus.Add(menuEn);
        }

        public void WithSubjects()
        {
            string[] subjects1 = {"Analiza 1", "Algebra", "Podstawy programowania strukturalnego"};
            string[] subjects1Names = {"analiza-1", "algebra", "podstawy-programowania-strukturalnego"};
            int i = 0;
            foreach (var subject in subjects1)
            {
                _context.Subjects.Add(new Subject
                {
                    Name = subject,
                    Semester = 1,
                    UrlName = subjects1Names[i++]
                });
            }
            string[] subjects2 = { "Metody Numeryczne 1", "Analiza 2", "Programowanie" };
            string[] subjects2Names = { "metody-numeryczne-1", "analiza-2", "programowanie" };
            i = 0;
            foreach (var subject in subjects2)
            {
                _context.Subjects.Add(new Subject
                {
                    Name = subject,
                    Semester = 2,
                    UrlName = subjects2Names[i++],
                    News = new List<Article>
                    {
                        new Article
                        {
                            Header = "Article 1",
                            Content = "Content",
                            PublishDate = DateTime.Now
                        },
                        new Article
                        {
                            Header = "Article 2",
                            Content = "Content",
                            PublishDate = DateTime.Now
                        },
                    }
                });
            }
        }

        public void WithRoles()
        {
            foreach (var roleName in RoleNames)
            {
                _roleManager.Create(new IdentityRole(roleName));
            }
        }

        public void WithUsers()
        {
            foreach (var user in Users)
            {
                _userManager.Create(user, user.PasswordHash);
            }
        }

        public void WithAdmin(string userName)
        {
            var admin = _userManager.Users
                        .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            if (admin != null)
            {
                _userManager.AddToRole(admin.Id, AdminRole);
            }
        }
    }
}