using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using BookBarn.Data;
using BookBarn.Models.SearchViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using BookBarn.Utilities;

namespace BookBarn.Controllers
{
    public class SaleItemsController : Controller
    {
        private readonly InitialModelsContext _context;
        private readonly AuthenticationContext _Acontext;

        public SaleItemsController(InitialModelsContext context, AuthenticationContext Acontext)
        {
            _context = context;
            _Acontext = Acontext;
        }

        public string UserID()
        {
            return _Acontext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Id;
        }


        // GET: SaleItems
        public async Task<IActionResult> Index()
        {

            var temp = await _context.SaleItem.ToListAsync();
            var viewList = from a in temp
                        where a.UserKey == UserID()
                        select a;
           
           return View(viewList);
           //Select(c =>c.UserKey==UserID()).ToListAsync());
            //return View(await _context.SaleItem.ToListAsync());
        }

        // GET: SaleItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var saleItem = await _context.SaleItem
                .SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(saleItem);
        }

        // GET: SaleItems/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
              return View();
            }
            else
            {
              Response.StatusCode = 401;
              return View("NotLoggedIn");
            }
        }

        // POST: SaleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleItemId,Price,IsSold,Quality,Isbn,Image,UserKey,Title,Subtitle,Authors,Publisher,PublishedData,Description,Isbn10Or13,ImageLinks")] SaleItem saleItem, IFormFile files)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (files != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await files.CopyToAsync(memoryStream);
                        saleItem.Image = memoryStream.ToArray();
                    }
                }
                if (Isbn.IsValidIsbn(saleItem.Isbn))
                {
                    saleItem.IsSold = false;
                    saleItem.UserKey = UserID();
                    _context.Add(saleItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid ISBN");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(saleItem);
        }

        // GET: SaleItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var saleItem = await _context.SaleItem.SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(saleItem);
        }

        // POST: SaleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleItemId,Price,IsSold,Quality,Isbn,Image,UserKey,Title,Subtitle,Authors,Publisher,PublishedData,Description,Isbn10Or13,ImageLinks")] SaleItem saleItem)
        {
            if (id != saleItem.SaleItemId)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleItemExists(saleItem.SaleItemId))
                    {
                      Response.StatusCode = 404;
                      return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(saleItem);
        }

        // GET: SaleItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var saleItem = await _context.SaleItem
                .SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(saleItem);
        }

        // POST: SaleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saleItem = await _context.SaleItem.SingleOrDefaultAsync(m => m.SaleItemId == id);
            _context.SaleItem.Remove(saleItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleItemExists(int id)
        {
            return _context.SaleItem.Any(e => e.SaleItemId == id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchType, string searchString, string sortType, string title, string author, string isbn, string quality, float minPrice, float maxPrice)
        {
            SearchViewModel searchVm;

            var resultSet = from si in _context.SaleItem
                            join b in _context.Book on si.Isbn equals b.Isbn
                            select new SearchResultViewModel
                            {
                                Title = b.Title,
                                Author = b.Author,
                                Quality = si.Quality.ToString(),
                                Price = si.Price,
                                ISBN = b.Isbn,
                                SaleItemID = si.SaleItemId,
                                Image = si.Image
                            };
            //Filter type
            if (!string.IsNullOrWhiteSpace(searchString) && !string.IsNullOrEmpty(searchType))
            {
                if (searchType.Equals("title"))
                {
                    resultSet = resultSet.Where(sr => sr.Title.ToLowerInvariant().Contains(searchString.ToLower()));
                }
                else if (searchType.Equals("author"))
                {
                    resultSet = resultSet.Where(sr => sr.Author.ToLowerInvariant().Contains(searchString.ToLower()));
                }
                else if (searchType.Equals("isbn"))
                {
                    resultSet = resultSet.Where(sr => sr.ISBN.Contains(searchString));
                }
                else
                {
                    throw new NotImplementedException("The current search type is not defined");
                }


            }

            //Sort option
            if (!String.IsNullOrEmpty(sortType))
            {
                if (sortType.Equals("price"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.Price);
                }
                if (sortType.Equals("title"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.Title);
                }
                if (sortType.Equals("author"))
                {
                    resultSet = resultSet.OrderBy(sr => sr.Author);
                }
            }

            //Advanced search
            if (!String.IsNullOrWhiteSpace(title))
            {
                resultSet = resultSet.Where(sr => sr.Title.ToLowerInvariant().Contains(title.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(author))
            {
                resultSet = resultSet.Where(sr => sr.Author.ToLowerInvariant().Contains(author.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(isbn))
            {
                resultSet = resultSet.Where(sr => sr.ISBN.ToLowerInvariant().Contains(isbn.ToLower()));
            }
            if (!String.IsNullOrEmpty(quality))
            {
                resultSet = resultSet.Where(sr => sr.Quality.Contains(quality));
            }
            if (!float.IsNaN(minPrice))
            {
                resultSet = resultSet.Where(sr => sr.Price >= minPrice);
            }
            if (!float.IsNaN(maxPrice) && maxPrice > 0 && maxPrice >= minPrice)
            {
                resultSet = resultSet.Where(sr => sr.Price <= maxPrice);
            }

            searchVm = new SearchViewModel()
            {
                SearchResults = await resultSet.ToListAsync()
            };

            return View(searchVm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AdvancedSearch(string searchType, string searchString, string sortType, string titleString)
        {
            SearchViewModel searchVm;
            searchVm = new SearchViewModel();

            return View(searchVm);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
