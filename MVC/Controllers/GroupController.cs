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
    // [Route("[controller]")]
    public class GroupController : Controller
    {
        private readonly IGroupInterface _groupRepo;

        public GroupController(IGroupInterface groupRepo)
        {
            _groupRepo = groupRepo;
        }

        // [HttpPost]
        // public async Task<IActionResult> Join(string groupName)
        // {
        //     var user = HttpContext.Session.GetString("username");

        //     await _groupRepo.JoinGroup(groupName, user);

        //     return Ok();
        // }

        [HttpPost]
        public async Task<IActionResult> Send(string groupName, string message)
        {
            var sender = HttpContext.Session.GetString("username");

            await _groupRepo.SendGroupMessage(groupName, sender, message);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Receive(string groupName)
        {
            var user = HttpContext.Session.GetString("username");

            var msg = await _groupRepo.ReceiveGroupMessage(groupName, user);

            if (msg == null)
                return Json(new { hasMessage = false });

            return Json(new { hasMessage = true, content = msg });
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}