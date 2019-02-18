using Diss.Models;
using Diss.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
                Finish = e.Finish
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
            return Json(myDbContext.Events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChatID([FromBody] Event e)
        {
            MyDbContext myDbContext = new MyDbContext();
            var game = myDbContext.Events.Find(e.ID);
            game.ChatID = e.ChatID;
            myDbContext.SaveChanges();
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }
    }
}