using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Diss
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Events",
                url: "",
                defaults: new { controller="events", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Chat",
                url: "chat/{eventId}",
                defaults: new { controller = "events", action = "Chat", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EventsJson",
                url: "events",
                defaults: new { controller = "events", action = "Events" }
            );

            routes.MapRoute(
                name: "ChatID",
                url: "chatID",
                defaults: new { controller = "events", action = "ChatID" }
            );

            routes.MapRoute(
                name: "Login_GET",
                url: "login",
                defaults: new { controller = "users", action = "Login_GET" }
            );

            routes.MapRoute(
                name: "Login_POST",
                url: "login-post",
                defaults: new { controller = "users", action = "Login_POST" }
            );

            routes.MapRoute(
                name: "Register_GET",
                url: "register",
                defaults: new { controller = "users", action = "Register_GET" }
            );

            routes.MapRoute(
                name: "Register_POST",
                url: "register-post",
                defaults: new { controller = "users", action = "Register_POST" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "users", action = "Logout" }
            );
        }
    }
}

