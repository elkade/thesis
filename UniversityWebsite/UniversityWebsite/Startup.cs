using Microsoft.Owin;
using Owin;
using UniversityWebsite;

[assembly: OwinStartup(typeof(Startup))]
namespace UniversityWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}