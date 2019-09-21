using System.Web.Mvc;
using System.Web.Routing;

namespace Sistema.Stoque.v1.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Usuario", action = "LoginUsuario", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Reset",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Usuario", action = "Editar", id = UrlParameter.Optional }
           );
        }
    }
}
