using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Core.Migrations
{
    public class InitialDataLoader
    {
        private readonly static string AdminRole = "Administrator";
        private static readonly string Admin = "su@su.su";
        private readonly static string[] RoleNames = { AdminRole, "Student", "Teacher" };
        private readonly ApplicationUser[] Users =
        {
            new ApplicationUser
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
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(domainContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(domainContext));
        }

        public void WithDefault()
        {
            WithRoles();
            WithUsers();
            WithAdmin(Admin);
            AddPagesAndMenus();

            _context.SaveChanges();
        }

        private void AddPagesAndMenus()
        {
            var pl = new Language {Name = "polski", CountryCode = "pl"};
            var en = new Language {Name = "english", CountryCode = "en"};
            var pagesPl = new List<Page>
            {
                new Page
                {
                    Title = "Kontakt",
                    UrlName = "Kontakt",
                    LangGroup = 1,
                    Language = pl
                },
                new Page
                {
                    Title = "Badania",
                    UrlName = "Badania",
                    Language = pl,
                    LangGroup = 2,
                },
                new Page
                {
                    Title = "Kadra",
                    UrlName = "Kadra",
                    Language = pl,
                    LangGroup = 3,
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
                    LangGroup = 1,
                },
                new Page
                {
                    Title = "Research",
                    Language = en,
                    UrlName = "Research",
                    LangGroup = 2,
                },
                new Page
                {
                    Title = "Staff",
                    UrlName = "Staff",
                    Language = en,
                    LangGroup = 3,
                }
            };
            foreach (var p in pagesPl)
                _context.Pages.Add(p);

            var menu1 = new NavigationMenu { Language = pl, Items = pagesPl };
            var menu2 = new NavigationMenu { Language = en, Items = pagesEn };

            _context.Menus.Add(menu1);
            _context.Menus.Add(menu2);

            _context.Phrases.Add(new Phrase { Group = "witaj", Language = pl, Text = "Witaj!" });
            _context.Phrases.Add(new Phrase { Group = "witaj", Language = en, Text = "Welcome!" });

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