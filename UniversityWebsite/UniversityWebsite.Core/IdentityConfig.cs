using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Core
{
    /// <summary>
    /// Manager zarządzający użytkownikami systemu.
    /// </summary>
    public class ApplicationUserManager : UserManager<User>
    {
        private const string _superUserLogin = "su@su.su";

        /// <summary>
        /// Nazwa super administratora systemu, którego nie można modyfikować.
        /// </summary>
        public string SuperUserLogin
        {
            get { return _superUserLogin;}
        }

        /// <summary>
        /// Id super administratora systemu, którego nie można modyfikować.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public string SuperUserId
        {
            get
            {
                var su = this.FindByName(_superUserLogin);
                if(su!=null)
                    return su.Id;
                throw new Exception("Cannot find superuser");
            }
        }

        /// <summary>
        /// Tworzy nową instancję managera.
        /// </summary>
        /// <param name="store">Opcje tworzenia managera</param>
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
                RequireLowercase = true,
            };
        }

        /// <summary>
        /// Tworzy nową instancję managera.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context">Kontekst OWIN</param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<User>(context.Get<DomainContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
                RequireLowercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
    /// <summary>
    /// Manager zarządzający uwierzytelnianiem użytkowników systemu.
    /// </summary>
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        /// <summary>
        /// Tworzy nową instancję managera.
        /// </summary>
        /// <param name="userManager">Manager zarządzający użytkownikami systemu.</param>
        /// <param name="authenticationManager">Podstawowy manager zarządzający uwierzytelnianiem użytkowników systemu.</param>
        private ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        /// <summary>
        /// Tworzy nową instancję managera.
        /// </summary>
        /// <param name="options">Opcje tworzenia managera</param>
        /// <param name="context">Kontekst OWIN</param>
        /// <returns></returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

}