using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using BookBarn.Models.SearchViewModels;


namespace BookBarn.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
<<<<<<< HEAD

            return View();
=======
            var searchVM = new SearchViewModel();
            return View(searchVM);
>>>>>>> master
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
