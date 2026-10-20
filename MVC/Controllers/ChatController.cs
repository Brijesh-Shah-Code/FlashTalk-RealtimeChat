using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Filters;
using Repositories.Interfaces;

namespace MVC.Controllers
{
    [SessionCheck]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;

        private readonly IUserInterface _userRepo;

        private readonly IMessageInterface _Msgrepo;

        public ChatController(ILogger<ChatController> logger, IMessageInterface msgrepo, IUserInterface userRepo)
        {
            _logger = logger;
            _Msgrepo = msgrepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllUsers();
            // Exclude the current logged-in user from the "Friends" list
            var currentEmail = HttpContext.Session.GetString("useremail");
            var friends = users.Where(u => u.c_Email != currentEmail).ToList();

            return View(friends);
        }


        [HttpPost]
        public async Task<IActionResult> Send(string toEmail, string message)
        {
            // Ensure toEmail is not null/empty before passing it to the repo
            if (string.IsNullOrEmpty(toEmail)) return BadRequest("Invalid recipient");

            await _Msgrepo.SendMessage(toEmail, message);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Receive()
        {
            var myEmail = HttpContext.Session.GetString("useremail");
            var msg = await _Msgrepo.ReceiveMessage(myEmail);

            return Json(new { hasMessage = msg != null, content = msg });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}