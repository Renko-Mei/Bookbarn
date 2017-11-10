using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace final_project.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("[action]")]
        public string SearchResult()
        {
            string searchString = Request.QueryString.Value.Substring(12);
            string searchQuery = "SELECT * FROM public.\"Book\" WHERE \"Title\" LIKE \'%" + searchString + "%\'";
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            string test = "";
            using (NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432; Database=bookdb; Username=postgres; Password=password"))
            {
                conn.Open();
                NpgsqlCommand query = new NpgsqlCommand(searchQuery, conn);
                NpgsqlDataReader dr = query.ExecuteReader();

                while (dr.Read())
                {
                    test = test + " " + dr[0] + " " + dr[1];
                }
                      
            }

            return test; //Enumerable.Range(1, 5);
        }
    }
}
