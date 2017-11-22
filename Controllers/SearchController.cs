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
using BookBarn.Models;
using BookBarn.Data;
using BookBarn.Models.SearchViewModels;

namespace BookBarn.Controllers
{
    public class SearchController : Controller
    {

        private readonly InitialModelsContext _context;

        public SearchController(InitialModelsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchType, string id, string sortType)
        {
            var resultSet = from si in _context.SaleItem
                            join b in _context.Book on si.BookId equals b.BookId
                            select new SearchResultViewModel
                            {
                                Title = b.Title,
                                AuthorFirst = b.AuthorFirstName,
                                AuthorLast = b.AuthorLastName,
                                Author = b.AuthorFirstName + " " + b.AuthorLastName,
                                Quality = si.Quality,
                                Price = si.Price,
                                ISBN = b.Isbn,
                                SaleItemID = si.SaleItemId
                            };
            //Filter type
            if (!String.IsNullOrEmpty(searchType) && !String.IsNullOrEmpty(id))
            {  
                if (searchType.Equals("title"))
                {
                    resultSet = resultSet.Where(sr => sr.Title.ToLower().Contains(id.ToLower()));
                }
                else if (searchType.Equals("author"))
                {
                    resultSet = resultSet.Where(sr => sr.Author.ToLower().Contains(id.ToLower()));
                }
                else if (searchType.Equals("isbn"))
                {
                    resultSet = resultSet.Where(sr => sr.ISBN.Contains(id));
                }
            }

            //Sort option
            if (!String.IsNullOrEmpty(sortType))
            {
                if (sortType.Equals("price"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.Price);
                }
                else if (sortType.Equals("title"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.Title);
                }
                else if (sortType.Equals("authorFirst"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.AuthorFirst);
                }
                else if (sortType.Equals("authorLast"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.AuthorLast);
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
