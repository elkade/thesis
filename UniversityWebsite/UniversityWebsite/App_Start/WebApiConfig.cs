using System.Linq;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Owin;
using UniversityWebsite.Filters;

namespace UniversityWebsite
{
    /// <summary>
    /// Odpowiada za konfigurację kontenera Dependency Injection Autofac dotyczącą modułu api
    /// </summary>
    public class WebApiConfig
    {
        /// <summary>
        /// Rejestruje Kontrolery, na których zastosowane zostanie wstrzykiwanie zależności.
        /// </summary>
        /// <param name="app">Obiekt reprezentujący dane palikacji</param>
        public static void Register(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new CustomExceptionFilterAttribute());
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            var container = AutofacConfig.Container;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}