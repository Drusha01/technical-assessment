using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace technical_assessment
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Recyclables",
               url: "recyclables/{action}",
               defaults: new { controller = "Recyclables", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "RecyclableItems",
              url: "recyclableitems/{action}",
              defaults: new { controller = "RecyclableItems", action = "Index", id = UrlParameter.Optional }
          );
        }
    }
}
