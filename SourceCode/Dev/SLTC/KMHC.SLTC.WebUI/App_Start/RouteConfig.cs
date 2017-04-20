using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KMHC.SLTC.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.html");

            routes.MapRoute(
            name: "AppNCIP",
            url: "NCIP/{*url}",
            defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
             name: "AppNCIA",
             url: "NCIA/{*url}",
             defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
             name: "AppNCI",
             url: "NCI/{*url}",
             defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Agency", id = UrlParameter.Optional }
            );
        }
    }
}