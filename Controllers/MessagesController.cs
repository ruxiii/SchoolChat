using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Models;
using SchoolChatOriginal.Data;
using SchoolChatOriginal.Models;
using Microsoft.AspNetCore.Authorization;


namespace SchoolChat.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MessagesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var messages=db.Messages.ToList();
            //List<Message>messages = new List<Message>();
            ViewBag.Messages = messages;
            return View();
        }

        //public IActionResult New() {
        //    return View();
        //}

        public IActionResult New(Message m) {
            try
            {
                int gid = (int)TempData["group_id"];
                m.IdGroup = gid;
                m.IdUser = TempData["userId"].ToString();
                m.MessageTime = System.DateTime.Now;
                m.Group = db.Groups.Find(gid);
                db.Messages.Add(m);
                db.SaveChanges();
                TempData["group_id"] = gid;
                return RedirectToRoute(new { controller = "SchoolGroups", action = "Communicate" });
            }
            
            catch
            {
                int gid = (int)TempData["group_id"];
                TempData["group_id"] = gid;
                return RedirectToRoute(new { controller = "SchoolGroups", action = "Communicate" });
            }
        }

        public IActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id);
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, Message requestMessage)
        {
            Message message = db.Messages.Find(id);
            try
            {
                message.TextMessage = requestMessage.TextMessage;
                db.SaveChanges();

                return RedirectToRoute(new { controller = "SchoolGroups", action = "Communicate" });
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", message.IdMessage);
            }
        }

        public IActionResult Delete(int id)
        {
            int gid = (int)TempData["group_id"];
            Message m = db.Messages.Find(id);
            db.Messages.Remove(m);
            db.SaveChanges();
            TempData["group_id"] = gid;
            return RedirectToRoute(new { controller = "SchoolGroups", action = "Communicate" });
        }
    }
}


