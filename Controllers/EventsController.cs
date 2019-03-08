using Diss.Models;
using Diss.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Diss.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            MyDbContext myDbContext = new MyDbContext();
            var events = myDbContext.Events;
            var model = events.Select(e => new EventView
            {
                ID = e.ID,
                Name = e.Name,
                Start = e.Start,
                Finish = e.Finish,
                Username = System.Web.HttpContext.Current.User.Identity.Name
            });

            return View(model);
        }

        // GET: Events
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Events()
        {
            MyDbContext myDbContext = new MyDbContext();
            return Json(myDbContext.Chatrooms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChatID([FromBody] EventView e)
        {
            using (var context = new MyDbContext())
            {
                var game = context.Events.Find(e.ID);

                context.Chatrooms.Add(new Chatroom
                {
                    ChatID = e.ChatID,
                    EventID = e.ID
                });

                context.SaveChanges();
            }
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }
    }
}