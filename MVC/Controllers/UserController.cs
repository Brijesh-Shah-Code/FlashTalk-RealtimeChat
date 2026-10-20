using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserInterface _repo;

        public UserController(ILogger<UserController> logger, IUserInterface repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            var user = await _repo.Login(email, password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("userid", user.c_UserId);
                HttpContext.Session.SetString("useremail", user.c_Email);
                HttpContext.Session.SetString("username", user.c_UserName); 

                return Json(new { success = true, message = "Login Successful" });
            }

            return Json(new { success = false, message = "Invalid Credentials" });
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}