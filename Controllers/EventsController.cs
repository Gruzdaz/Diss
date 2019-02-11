using Diss.Models;
using Diss.ViewModels;
using System.Web.Mvc;

namespace Diss.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            MyDbContext myDbContext = new MyDbContext();
            var events = myDbContext.Events.Find(1);
            var model = new EventView
            {
                Name = events.Name,
                Start = events.Start,
                Finish = events.Finish
            };

            return View(model);
        }

        // GET: Events
        public ActionResult Chat()
        {
            return View();
        }
    }
}