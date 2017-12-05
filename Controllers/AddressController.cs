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
        private readonly AuthenticationContext _Acontext;

        public AddressController(InitialModelsContext context, AuthenticationContext Acontext)
        {
            _context = context;
            _Acontext = Acontext;
        }

        public string UserID()
        {
            return _Acontext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Id;
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            var temp = await _context.Address.ToListAsync();

            var viewList = from a in temp 
                        where a.UserKey == UserID() 
                        select a;

            if (User.Identity.IsAuthenticated)
            {
              return View(viewList);
            }
            else
            {
              Response.StatusCode = 401;
              return View("NotLoggedIn");
            }
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
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
                if(address.UserKey==UserID()){
                    return View(address);
                }
                else{
                    return View("NoAccess");
                } 
            }
            else
            {
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }
        }





        // GET: Address/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                var temp = await _context.Address.ToListAsync();

                var viewList = from a in temp 
                        where a.UserKey == UserID() 
                        select a;
                if(viewList.Count() ==1){
                    return View("AddressLimit");
                }
                else{
                    return View();
                }

                
            }
            else
            {
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,LegalName,StreetAddress,City,Province,Country,PostalCode,PhoneNumber,UserKey")] Address address)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                address.UserKey = UserID();
                _context.Add(address);
                await _context.SaveChangesAsync();
                return View("AddressCreateSuccess");
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
            if(address.UserKey==UserID())
            {
                return View(address);
            }
            else
            {
                return View("NoAccess");
            }
            
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
                    address.UserKey = UserID();
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
            if(User.Identity.IsAuthenticated)
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
            else
            {
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }
            
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
