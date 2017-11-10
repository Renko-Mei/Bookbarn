using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections;
using final_project.Models;

namespace final_project.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index(string id)
        {

            var searchVM = new SearchViewModel();
            List<string> titles = new List<string>();
            List<string> authors = new List<string>();

            string searchString = id;
            if (!String.IsNullOrEmpty(searchString))
            {
                string searchQuery = "SELECT * FROM public.\"Book\" WHERE \"Title\" LIKE \'%" + searchString + "%\'";
                List<string[]> queryResult = new List<string[]>();

                using (NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432; Database=bookdb; Username=postgres; Password=password"))
                {
                    conn.Open();
                    NpgsqlCommand query = new NpgsqlCommand(searchQuery, conn);
                    NpgsqlDataReader dr = query.ExecuteReader();

                    while (dr.Read())
                    {
                        string[] container = { dr[2].ToString(), dr[3].ToString() };
                        queryResult.Add(container);
                        titles.Add(dr[2].ToString());
                        authors.Add(dr[3].ToString());
                    }

                }
            }
            searchVM.Title = titles;
            searchVM.Author = authors;
            return View(searchVM);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IEnumerable<SaleResult> SearchResult()
        {
            //<form action="/api/Search/SearchResult">
            string searchString = Request.QueryString.Value.Substring(12);
            string searchQuery = "SELECT * FROM public.\"Book\" WHERE \"Title\" LIKE \'%" + searchString + "%\'";
            List <string []> queryResult = new List<string[]>() ;
            using (NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432; Database=bookdb; Username=postgres; Password=password"))
            {
                conn.Open();
                NpgsqlCommand query = new NpgsqlCommand(searchQuery, conn);
                NpgsqlDataReader dr = query.ExecuteReader();

                while (dr.Read())
                {
                    //test = test + " " + dr[2] + " " + dr[3];
                    string[] container = {dr[2].ToString(), dr[3].ToString()};
                    queryResult.Add(container);
                }
                      
            }

            return Enumerable.Range(1, queryResult.Count).Select(index => new SaleResult
            {
                Title = queryResult.ElementAt(index)[0],
                Author = queryResult.ElementAt(index)[1]
            });
        }

        public class SaleResult
        {
            public string Title { get; set; }
            public string Author { get; set; }

        }
    }
}
