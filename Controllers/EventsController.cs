using Diss.Models;
using Diss.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System;

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
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Users");
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
                    EventID = e.ID,
                    PeopleCount = 1,
                    AverageELO = context.Users.Where(u => HttpContext.User.Identity.Name == u.UserName).Select(u => u.ELO).FirstOrDefault(),
                    MixedRoom = e.MixedRoom,
                    HomeTeamRoom = e.HomeTeamRoom
                });

                context.SaveChanges();
            }
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateChat([FromBody] ChatroomView e)
        {
            using (var context = new MyDbContext())
            {

                var user = HttpContext.User.Identity.Name;

                var chatroom = context.Chatrooms.Where(c => c.ChatID == e.ChatID && c.EventID == e.EventID).FirstOrDefault();

                var userELO = context.Users.Where(u => user == u.UserName).Select(u => u.ELO).FirstOrDefault();
                if (!e.Users.Contains(user)){
                    chatroom.PeopleCount += 1;
                    chatroom.AverageELO = (chatroom.AverageELO + userELO) / chatroom.PeopleCount;
                }

                context.SaveChanges();
            }
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MatchRoom([FromBody] PreferencesView e)
        {
            using (var context = new MyDbContext())
            {

                var user = HttpContext.User.Identity.Name;
                var userELO = context.Users.Where(u => user == u.UserName).Select(u => u.ELO).FirstOrDefault();

                var chatrooms = context.Chatrooms.Where(c => c.EventID == e.EventID && c.PeopleCount < 10).ToList();

                if (!e.AllowsMixedRooms)
                {
                    chatrooms = chatrooms.Where(c => c.HomeTeamRoom == e.SupportsHomeTeam).ToList();
                }

                int closestELO = chatrooms.Select(c => c.AverageELO).Aggregate((x, y) => Math.Abs(x - userELO) < Math.Abs(y - userELO) ? x : y);

                var roomID = context.Chatrooms.Where(c => c.AverageELO == closestELO).FirstOrDefault();

                return Json(roomID, JsonRequestBehavior.AllowGet);
            }
        }
    }
}