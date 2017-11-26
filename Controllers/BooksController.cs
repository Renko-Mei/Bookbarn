using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using BookBarn.Data;
using BookBarn.Utilities;

namespace BookBarn.Controllers
{
    public class BooksController : Controller
    {
        private readonly InitialModelsContext _context;

        public BooksController(InitialModelsContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var book = await _context.Book
                .SingleOrDefaultAsync(m => m.Isbn == isbn);
            if (book == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Isbn,Title,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                if (Isbn.IsValidIsbn(book.Isbn))
                {
                    book.Isbn = Isbn.NormalizeIsbn(book.Isbn);
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid ISBN");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var book = await _context.Book.SingleOrDefaultAsync(m => m.Isbn == isbn);
            if (book == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string isbn, [Bind("BookId,Isbn,Title,Author")] Book book)
        {
            if (isbn != book.Isbn)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Isbn.IsValidIsbn(book.Isbn))
                    {
                        book.Isbn = Isbn.NormalizeIsbn(book.Isbn);
                        _context.Update(book);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Isbn))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var book = await _context.Book
                .SingleOrDefaultAsync(m => m.Isbn == isbn);
            if (book == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string isbn)
        {
            var book = await _context.Book.SingleOrDefaultAsync(m => m.Isbn == isbn);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string isbn)
        {
            return _context.Book.Any(e => e.Isbn == isbn);
        }
    }
}
