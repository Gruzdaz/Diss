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
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Events",
                url: "",
                defaults: new { controller="events", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CurrentUser",
                url: "CurrentUser",
                defaults: new { controller = "users", action = "CurrentUser", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Chat",
                url: "chat/{eventId}/{title}",
                defaults: new { controller = "events", action = "Chat", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UsersJson",
                url: "users",
                defaults: new { controller = "users", action = "AllUsers" }
            );

            routes.MapRoute(
                name: "MatchRoom",
                url: "MatchRoom",
                defaults: new { controller = "events", action = "MatchRoom" }
            );

            routes.MapRoute(
                name: "UpdateChat",
                url: "UpdateChat",
                defaults: new { controller = "events", action = "UpdateChat" }
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
                name: "Login",
                url: "login",
                defaults: new { controller = "users", action = "Login" }
            );

            routes.MapRoute(
                name: "Registe",
                url: "register",
                defaults: new { controller = "users", action = "Register" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "users", action = "Logout" }
            );
        }
    }
}

