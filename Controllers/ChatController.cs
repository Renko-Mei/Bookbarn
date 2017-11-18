using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace BookBarn.Controllers
{
    public class ChatController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View("UserName");
        }

        [HttpPost]
        public IActionResult Index(string username)
        {
            return View("Index", username);
        }
    }
}
