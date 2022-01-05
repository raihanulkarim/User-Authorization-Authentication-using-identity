using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebProjectOnAuthorization.Data;
using WebProjectOnAuthorization.Models;
using WebProjectOnAuthorization.ViewModels;

namespace WebProjectOnAuthorization.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<ApplicationUser> userManager,ApplicationDbContext _context,ILogger<HomeController> logger)
        {
            this.userManager = userManager;
            context = _context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userManager.GetUserId(HttpContext.User));
            if (user.IsProfileComplete == false)
            {
                return RedirectToAction("welcome");
            }
            ViewBag.FirstName = user.FirstName;
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {
            var users = (from user in userManager.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Email = user.Email,
                                      PropPic = user.ProPic,
                                      IsDisable = user.IsDisable,
                                      IsCompleted = user.IsProfileComplete
                                      //RoleNames = (from userRole in user.Roles 
                                                  // join role in context.Roles on userRole.RoleId
                                                   //equals role.Id
                                                   //select role.Name).ToList()
                                  }).ToList().Skip(1).Select(p => new UserListViewModel()

                                  {
                                      UserId = p.UserId,
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      Email = p.Email,
                                      IsDisable = p.IsDisable,
                                      ProPic = p.PropPic,
                                      IsCompleted = p.IsCompleted
                                      //Role = string.Join(",", p.RoleNames)
                                  });;
            var noU = userManager.Users.Count();
            if (noU == 1)
            {
                ViewBag.noUser = true;
            }
            return View(users);
        }
        public async Task<IActionResult> Welcome()
        {
            
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userManager.GetUserId(HttpContext.User));
            ViewBag.FirstName = user.FirstName;
            if (user.IsProfileComplete == true)
            {
                return RedirectToAction("index");
            }
            return View();
        }
        public IActionResult DisableUser()
        {
            return View();
        }
        public async Task<IActionResult> LoginRedirect()
        {
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userManager.GetUserId(HttpContext.User));
            if (user.IsProfileComplete == true)
            {
                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("Welcome", "Home");

            }
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
