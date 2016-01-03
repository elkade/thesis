using Microsoft.Owin;
using Owin;
using UniversityWebsite;

[assembly: OwinStartup(typeof(Startup))]
namespace UniversityWebsite
{
    public partial class Startup
    {
        /// <summary>
        /// Wywołuje metody konfigurujące uwierzytelnianie w systemie i rejestrujące moduł api w kontenerze IOC
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            WebApiConfig.Register(app);
        }
    }
}