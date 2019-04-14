using Diss.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diss.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return Redirect("/" ?? Url.Action("Index", "Events"));
            }
            return View(new ViewModels.LoginView { });
        }

        [HttpPost]
        public ActionResult Login(ViewModels.LoginView model)
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
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Events");
            }
            return View(new ViewModels.RegisterView { });
        }

        [HttpPost]
        public ActionResult Register(ViewModels.RegisterView model)
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
                TempData["message"] = "wrong";
                return View(new ViewModels.RegisterView { ErrorMessage = result.Errors.First() });
            }
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["message"] = "Logout";
            return RedirectToAction("Index", "Events");
        }

        public ActionResult CurrentUser()
        {
            var user = HttpContext.User.Identity.Name;
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllUsers()
        {
            MyDbContext myDbContext = new MyDbContext();
            return Json(myDbContext.Users.Select(u => u.UserName).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}