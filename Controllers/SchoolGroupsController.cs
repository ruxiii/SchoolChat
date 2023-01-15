using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolChat.Models;
using SchoolChatOriginal.Data;
using SchoolChatOriginal.Models;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Build.Globbing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SchoolChat.Controllers
{
    public class SchoolGroupsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SchoolGroupsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //[Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            int _perPage = 5;

            var groups = db.Groups
                           .Include("Category")
                           .OrderBy(a => a.GroupName);

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere

                // Cautare in articol (Title si Content)

                List<int> SchoolGroupIds = db.Groups.Where
                                        (
                                         at => at.GroupName.Contains(search)
                                         || at.GroupDescription.Contains(search)
                                        ).Select(a => a.IdGroup).ToList();



                // Cautare in comentarii (Content)
                List<int> SelectedCategories = db.Categories
                                        .Where
                                        (
                                         c => c.CategoryName.Contains(search)
                                        ).Select(c => (int)c.Id).ToList();

                List<int> SchoolGroupIdsOfCategory = db.Groups.Where(a => SelectedCategories
                                                    .Contains((int)a.IdCategory))
                                                    .Select(g => g.IdGroup).ToList();



                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<int> mergedIds = SchoolGroupIds.Union(SchoolGroupIdsOfCategory).ToList();




                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                groups = db.Groups.Where(g => mergedIds.Contains(g.IdGroup))
                                      .Include("Category")
                                      .OrderBy(a => a.GroupName);
            }

            ViewBag.SearchString = search;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = groups.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedGroups = groups.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.SchoolGroups = paginatedGroups;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/Index/?page";
            }

            if (_userManager.GetUserId(User) != null)
            {
                string usr_id = _userManager.GetUserId(User).ToString();

                bool noreqsent;

                ViewBag.ModeratorInGroupIds = db.UserGroups.Where(ug => ug.IdUser == usr_id && ug.IsModerator == true)
                                                           .Select(ug => ug.IdGroup)
                                                           .ToList();

                if (db.GroupRequests.Where(gr => gr.IdUser == usr_id).Count() > 0)
                {
                    ViewBag.GroupsRequestedIds = db.GroupRequests.Where(gr => gr.IdUser == usr_id).Select(gr => gr.IdGroup).ToList();

                    noreqsent = false;
                }

                else
                {
                    noreqsent = true;
                }

                TempData["NoRequestsSent"] = noreqsent;
                //TempData["congrats_reason"] = noreqsent;
                //return RedirectToAction("Done");
            }

            //ViewBag.NoRequestsSent = true;

            return View();
        }

        public IActionResult AnaAreMere()
        {
            string user_id = _userManager.GetUserId(User).ToString();

            ViewBag.ModeratorInGroupIds = db.UserGroups.Where(ug => ug.IdUser == user_id && ug.IsModerator == true)
                                                           .Select(ug => ug.IdGroup)
                                                           .ToList();

            int _perPage = 5;

            var group_ids = db.UserGroups.Where(ug => ug.IdUser == user_id).Select(ug => ug.IdGroup).ToList();

            var groups = db.Groups.Where(g => group_ids.Contains(g.IdGroup)).OrderBy(g => g.GroupName);

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere

                // Cautare in articol (Title si Content)

                List<int> SchoolGroupIds = db.Groups.Where
                                        (
                                         at => at.GroupName.Contains(search)
                                         || at.GroupDescription.Contains(search)
                                        ).Select(a => a.IdGroup).ToList();



                // Cautare in comentarii (Content)
                List<int> SelectedCategories = db.Categories
                                        .Where
                                        (
                                         c => c.CategoryName.Contains(search)
                                        ).Select(c => (int)c.Id).ToList();

                List<int> SchoolGroupIdsOfCategory = db.Groups.Where(a => SelectedCategories
                                                    .Contains((int)a.IdCategory))
                                                    .Select(g => g.IdGroup).ToList();



                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<int> mergedIds = SchoolGroupIds.Union(SchoolGroupIdsOfCategory).ToList();




                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                groups = db.Groups.Where(g => mergedIds.Contains((int)g.IdGroup))
                                  .OrderBy(a => a.GroupName);
            }

            ViewBag.SearchString = search;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = groups.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedGroups = groups.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.SchoolGroups = paginatedGroups;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/AnaAreMere/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/AnaAreMere/?page";
            }

            return View();
        }

        public IActionResult Opportunities()
        {
            string user_id = _userManager.GetUserId(User);

            int _perPage = 5;

            var group_ids = db.UserGroups.Where(ug => ug.IdUser == user_id).Select(ug => ug.IdGroup).ToList();

            var groups = db.Groups.Where(g => !group_ids.Contains(g.IdGroup)).OrderBy(g => g.GroupName);

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere

                // Cautare in articol (Title si Content)

                List<int> SchoolGroupIds = db.Groups.Where
                                        (
                                         at => at.GroupName.Contains(search)
                                         || at.GroupDescription.Contains(search)
                                        ).Select(a => a.IdGroup).ToList();



                // Cautare in comentarii (Content)
                List<int> SelectedCategories = db.Categories
                                        .Where
                                        (
                                         c => c.CategoryName.Contains(search)
                                        ).Select(c => (int)c.Id).ToList();

                List<int> SchoolGroupIdsOfCategory = db.Groups.Where(a => SelectedCategories
                                                    .Contains((int)a.IdCategory))
                                                    .Select(g => g.IdGroup).ToList();



                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<int> mergedIds = SchoolGroupIds.Union(SchoolGroupIdsOfCategory).ToList();




                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                groups = db.Groups.Where(g => mergedIds.Contains((int)g.IdGroup))
                                  .OrderBy(a => a.GroupName);
            }

            ViewBag.SearchString = search;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = groups.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedGroups = groups.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.SchoolGroups = paginatedGroups;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/Opportunities/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/SchoolGroups/Opportunities/?page";
            }

            bool noreqsent;

            if (db.GroupRequests.Where(gr => gr.IdUser == user_id).Count() > 0)
            {
                ViewBag.GroupsRequestedIds = db.GroupRequests.Where(gr => gr.IdUser == user_id).Select(gr => gr.IdGroup).ToList();

                noreqsent = false;
            }

            else
            {
                noreqsent = true;
            }

            TempData["NoRequestsSent"] = noreqsent;

            return View();
        }

        public IActionResult Find()
        {
            return View();
        }

        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from cat in db.Categories
                             select cat;
            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            return selectList;
        }

        public IActionResult New()
        {
            SchoolGroup group = new SchoolGroup();
            group.Categ = GetAllCategories();
            return View(group);
        }

        [HttpPost]
        public IActionResult New(SchoolGroup g)
        {
            try
            {
                db.Groups.Add(g);
                db.SaveChanges();
                string user_id = _userManager.GetUserId(User);
                int group_id = g.IdGroup;
                db.UserGroups.Add(new UserGroup
                {
                    IdUser = user_id,
                    IdGroup = group_id,
                    IsModerator= true
                });
                db.SaveChanges();
                TempData["congrats_reason"] = "added";
                return RedirectToAction("Done");
            }
            catch(Exception e)
            {
                g.Categ = GetAllCategories();
                return View(g);
            }
        }

        public IActionResult Done()
        {
            if (TempData.ContainsKey("congrats_reason"))
            {
                ViewBag.message = TempData["congrats_reason"].ToString();
            }

            return View();
        }

        public IActionResult Show(int id)
        {
            SchoolGroup sch= db.Groups.Find(id);

            Category categ = db.Categories.Find(sch.IdCategory);

            var members_ids = db.UserGroups.Where(x => x.IdGroup == id).Select(x => x.IdUser);

            var members = from au in db.ApplicationUsers.Where(u => members_ids.Contains(u.Id))/*.Include("UserGroup")*/
                          select new
                          {
                              Id = au.Id,
                              UserName = au.UserName,
                              Status = (from ug in au.UserGroups
                                        where ug.IdGroup == id
                                        select ug.IsModerator
                                        ).First()
                          };

            var members_list = members.ToList();

            var member_requests_ids = db.GroupRequests.Where(x => x.IdGroup == id).Select(x => x.IdUser);

            var member_requests = db.ApplicationUsers.Where(u => member_requests_ids.Contains(u.Id));

            var member_requests_list = member_requests.ToList();

            ViewBag.Category = categ;

            ViewBag.Members = members_list;

            if (_userManager.GetUserId(User) != null)
            {
                string usr_id = _userManager.GetUserId(User).ToString();

                ViewBag.UserId = usr_id;

                var grp = (from ug in db.UserGroups
                           where (ug.IdGroup == id && ug.IdUser == usr_id)
                           select ug).Count();

                ViewBag.IsInGroup = grp;

                if (grp != 0)
                {
                    ViewBag.UserIsModerator = (from ug in db.UserGroups
                                               where (ug.IdGroup == id && ug.IdUser == usr_id)
                                               select ug.IsModerator).First()/*.Count()*/;
                }

                ViewBag.MembershipRequests = member_requests_list;

                if (db.GroupRequests.Where(gr => gr.IdUser == usr_id).Count() > 0)
                {
                    ViewBag.GroupsRequestedIds = db.GroupRequests.Where(gr => gr.IdUser == usr_id).Select(gr => gr.IdGroup).ToList();
                }
            }

            return View(sch);
        }
        
        public ActionResult Delete(int id)
        {
            SchoolGroup sch = db.Groups.Find(id);
            db.Groups.Remove(sch);
            List<UserGroup> ugs = db.UserGroups.Where(u => u.IdGroup == id).ToList();
            foreach (UserGroup u in ugs)
            {
                db.UserGroups.Remove(u);
            }
            List<Message> messages = db.Messages.Where(m => m.IdGroup == id).ToList();
            foreach (Message m in messages)
            {
                db.Messages.Remove(m);
            }
            List<GroupRequest> reqs = db.GroupRequests.Where(m => m.IdGroup == id).ToList();
            foreach (GroupRequest r in reqs)
            {
                db.GroupRequests.Remove(r);
            }
            db.SaveChanges();
            TempData["congrats_reason"] = "deleted";
            return RedirectToAction("Done");
        }

        public IActionResult Apply(int id)
        {
            string user_id = _userManager.GetUserId(User);
            int group_id = id;
            
            db.GroupRequests.Add(new GroupRequest
            {
                IdUser = user_id,
                IdGroup = group_id,
                //Intention = ""
            });
            db.SaveChanges();
            TempData["congrats_reason"] = "membership requested";
            return RedirectToAction("Done");
        }

        //[HttpPost]
        public IActionResult Enlarge(string id)
        {
            int group_id = (int)TempData["group_id_r"];

            GroupRequest gr = (db.GroupRequests.Where(u => u.IdGroup == group_id && u.IdUser == id)).First();
            db.GroupRequests.Remove(gr);
            db.UserGroups.Add(new UserGroup
            {
                IdUser = id,
                IdGroup = group_id,
                IsModerator = false
            });
            db.SaveChanges();
            TempData["congrats_reason"] = "membership accepted";
            //TempData["congrats_reason"] = user_id;
            return RedirectToAction("Done");
            //ViewBag.GroupId = group_id;
            //ViewBag.UserId = user_id;
            //return View();
        }

        public IActionResult Dismiss(string id)
        {
            int group_id = (int)TempData["group_id_r"];

            GroupRequest gr = db.GroupRequests.Where(u => u.IdGroup == group_id && u.IdUser == id).First();
            db.GroupRequests.Remove(gr);
            db.SaveChanges();
            TempData["congrats_reason"] = "membership rejected";
            return RedirectToAction("Done");
        }

        public ActionResult Abandon()
        {
            string user_id = TempData["user_id"].ToString();
            int group_id = (int) TempData["group_id"];
            UserGroup ug_rem = db.UserGroups.Where(ug => ug.IdGroup == group_id && ug.IdUser == user_id).First();

            db.UserGroups.Remove(ug_rem);
            db.SaveChanges();
            TempData["congrats_reason"] = "left";
            return RedirectToAction("Done");
        }

        public ActionResult Leave(string id)
        {
            int group_id = (int)TempData["group_id"];
            UserGroup ug_rem = db.UserGroups.Where(ug => ug.IdGroup == group_id && ug.IdUser == id).First();

            db.UserGroups.Remove(ug_rem);
            db.SaveChanges();
            TempData["congrats_reason"] = "participant removed";
            return RedirectToAction("Done");
        }

        public ActionResult Promote(string id)
        {
            int group_id = (int)TempData["group_id"];

            UserGroup old_ug=db.UserGroups.Where(ug => ug.IdGroup == group_id && ug.IdUser == id).First();
            UserGroup new_ug = new UserGroup{ 
                                                IdGroup=group_id,
                                                IdUser=id,
                                                IsModerator=true
                                             };
            db.UserGroups.Remove(old_ug);
            db.UserGroups.Add(new_ug);
            db.SaveChanges();
            TempData["congrats_reason"] = "moderator added";
            return RedirectToAction("Done");
        }

        public ActionResult Demote(string id)
        {
            int group_id = (int)TempData["group_id"];

            UserGroup old_ug = db.UserGroups.Where(ug => ug.IdGroup == group_id && ug.IdUser == id).First();
            UserGroup new_ug = new UserGroup
            {
                IdGroup = group_id,
                IdUser = id,
                IsModerator = false
            };
            db.UserGroups.Remove(old_ug);
            db.UserGroups.Add(new_ug);
            db.SaveChanges();
            TempData["congrats_reason"] = "moderator demoted";
            return RedirectToAction("Done");
        }

        public IActionResult Communicate() {
            string user_id = _userManager.GetUserId(User).ToString();
            ViewBag.IdUser = user_id;
            int group_id = (int)TempData["group_id"];
            ViewBag.IdGroup= group_id;
            //var msg = db.Messages.Include("ApplicationUser").Where(u => u.IdGroup == group_id).ToList();
            var msg = (from m in db.Messages.Where(u => u.IdGroup == group_id)
                      select new {
                          Id = m.IdMessage,
                          text = m.TextMessage,
                          time = m.MessageTime,
                          username = (from u in db.ApplicationUsers.Where(ui => ui.Id == m.IdUser)
                                      select u.UserName).First(),
                          own_message = m.IdUser == user_id
                      }).ToList();
            ViewBag.Messages = msg;
            return View(db.Groups.Find(group_id));
        
        }
    }
}


