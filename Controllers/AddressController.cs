using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using BookBarn.Data;

namespace BookBarn.Controllers
{
    public class AddressController : Controller
    {
        private readonly InitialModelsContext _context;

        public AddressController(InitialModelsContext context)
        {
            _context = context;
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            return View(await _context.Address.ToListAsync());
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var address = await _context.Address
                .SingleOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(address);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,LegalName,StreetAddress,City,Province,Country,PostalCode,PhoneNumber,UserKey")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var address = await _context.Address.SingleOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,LegalName,StreetAddress,City,Province,Country,PostalCode,PhoneNumber,UserKey")] Address address)
        {
            if (id != address.AddressId)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
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
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            var address = await _context.Address
                .SingleOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
              Response.StatusCode = 404;
              return View("NotFound");
            }

            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Address.SingleOrDefaultAsync(m => m.AddressId == id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }
    }
}
