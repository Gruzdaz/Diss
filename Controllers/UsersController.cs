using Diss.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Diss.Controllers
{
    public class UsersController : Controller
    {
        [System.Web.Http.HttpGet]
        public ActionResult Login_GET()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return Redirect("/" ?? Url.Action("Index", "Events"));
            }
            return View(new ViewModels.LoginView { });
        }

        [System.Web.Http.HttpPost]
        public ActionResult Login_POST(ViewModels.LoginView model)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;

                AppUser user = userManager.Find(model.Username, model.Password);
                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    //use the instance that has been created. 
                    authManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    TempData["message"] = "Logged In";
                    return RedirectToAction("Index", "Events");
                }
            }
            TempData["message"] = "wrong";
            return View("Login_GET");
        }

        [System.Web.Http.HttpGet]
        public ActionResult Register_GET()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Events");
            }
            return View(new ViewModels.RegisterView { });
        }

        [System.Web.Http.HttpPost]
        public ActionResult Register_POST(ViewModels.RegisterView model)
        {
            var store = new UserStore<AppUser>(new MyDbContext());
            AppUserManager _userManager = new AppUserManager(store);
            var manager = _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var user = new AppUser() {UserName = model.Username };
            var result = manager.Create(user, model.Password);

            if (result.Succeeded)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                TempData["message"] = "Registered";
                return RedirectToAction("Index", "Events");
            }
            else
            {

                return View("Register_GET", new ViewModels.RegisterView { ErrorMessage = result.Errors.First() });
            }
        }

        [System.Web.Http.HttpGet]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["message"] = "Logout";
            return RedirectToAction("Index", "Events");
        }
    }
}