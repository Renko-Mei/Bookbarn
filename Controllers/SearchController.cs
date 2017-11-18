using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;
using final_project.Models;
using final_project.Data;
using final_project.Models.SearchViewModels;

namespace final_project.Controllers
{
    public class SearchController : Controller
    {

        private readonly InitialModelsContext _context;

        public SearchController(InitialModelsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchType, string id)
        {
            var resultSet = from si in _context.SaleItem
                            join b in _context.Book on si.BookId equals b.BookId
                            select new SearchResultViewModel
                            {
                                Title = b.Title,
                                Author = b.AuthorFirstName + " " + b.AuthorLastName,
                                Quality = si.Quality,
                                Price = si.Price
                            };

            if (!String.IsNullOrEmpty(searchType))
            {
                if (searchType.Equals("title"))
                {
                    resultSet = resultSet.Where(sr => sr.Title.Contains(id));
                }
                else if (searchType.Equals("author"))
                {
                    resultSet = resultSet.Where(sr => sr.Author.Contains(id));
                }
            }

            var searchVM = new SearchViewModel();
            searchVM.SearchResults = await resultSet.ToListAsync();
            return View(searchVM);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

    }
}
