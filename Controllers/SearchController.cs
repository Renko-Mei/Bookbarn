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
        public IActionResult Index(string searchType, string id)
        {

            var searchVM = new SearchViewModel();
            List<string> titles = new List<string>();
            List<string> authors = new List<string>();
            List<string> price = new List<string>();
            List<string> quality = new List<string>();

            List<SearchResult> searchResultList = new List<SearchResult>();
            
            string searchString = id;
            string filter = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchType.Equals("title"))
                {
                    filter = "b.\"Title\"";
                }
                else if (searchType.Equals("author"))
                {
                    filter = "b.\"Author\"";
                }
                string searchQuery = "select b.\"Title\", b.\"Author\", si.\"Quality\", si.\"Price\", si.\"SaleItemID\" from public.\"SaleItem\" si inner join public.\"Book\" b on si.\"BookID\" = b.\"BookID\" WHERE " + filter + " LIKE \'%" + searchString + "%\'";
                List<string[]> queryResult = new List<string[]>();

                using (NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Port=5432; Database=bookdb; Username=postgres; Password=password"))
                {
                    conn.Open();
                    NpgsqlCommand query = new NpgsqlCommand(searchQuery, conn);
                    NpgsqlDataReader dr = query.ExecuteReader();

                    while (dr.Read())
                    {
                        //string[] container = { dr[2].ToString(), dr[3].ToString() };
                        //// queryResult.Add(container);
                        //titles.Add(dr[0].ToString());
                        //authors.Add(dr[1].ToString());
                        // quality.Add(dr[2].ToString());
                        // price.Add(dr[3].ToString());
                        var result = new SearchResult();
                        result.Title = dr[0].ToString();
                        result.Author = dr[1].ToString();
                        result.Quality = dr[2].ToString();
                        result.Price = dr[3].ToString();
                        result.SaleItemID =  (int) dr[4];
                        searchResultList.Add(result);
                    }

                }
            }
           // searchVM.Title = titles;
           // searchVM.Author = authors;
           // searchVM.Quality = quality;
            //searchVM.Price = price;

            searchVM.SearchResults = searchResultList;
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
