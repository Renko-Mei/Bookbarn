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

namespace BookBarn.Controllers
{
    public class SaleItemsController : Controller
    {
        private readonly InitialModelsContext _context;

        public SaleItemsController(InitialModelsContext context)
        {
            _context = context;
        }

        // GET: SaleItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.SaleItem.ToListAsync());
        }

        // GET: SaleItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleItem = await _context.SaleItem
                .SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
                return NotFound();
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
              // do this for now, need to change to not authorized TODO - ushma
              return NotFound();
            }
        }

        // POST: SaleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleItemId,Price,Quality,IsSold,BookId")] SaleItem saleItem)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                _context.Add(saleItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(saleItem);
        }

        // GET: SaleItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleItem = await _context.SaleItem.SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
                return NotFound();
            }
            return View(saleItem);
        }

        // POST: SaleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleItemId,Price,Quality,IsSold,BookId")] SaleItem saleItem)
        {
            if (id != saleItem.SaleItemId)
            {
                return NotFound();
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
                        return NotFound();
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
                return NotFound();
            }

            var saleItem = await _context.SaleItem
                .SingleOrDefaultAsync(m => m.SaleItemId == id);
            if (saleItem == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Search(string searchType, string searchString)
        {
            SearchViewModel searchVm;

            var resultSet = from si in _context.SaleItem
                            join b in _context.Book on si.BookId equals b.BookId
                            select new SearchResultViewModel
                            {
                                Title = b.Title,
                                Author = b.AuthorFirstName + " " + b.AuthorLastName,
                                Quality = si.Quality,
                                Price = si.Price,
                                ISBN = b.Isbn
                            };

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

            searchVm = new SearchViewModel()
            {
                SearchResults = await resultSet.ToListAsync()
            };

            return View(searchVm);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
