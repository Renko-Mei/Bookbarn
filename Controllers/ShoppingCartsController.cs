using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Data;
using BookBarn.Models;
using BookBarn.Models.ShoppingCartViewModel;

namespace BookBarn.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly InitialModelsContext _context;
        private readonly ShoppingCart _shoppingCart;


        public ShoppingCartsController(InitialModelsContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        // // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var shoppingCartView = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartView);
            //return View(await _context.ShoppingCart.ToListAsync());
        }
  
        public RedirectToActionResult AddToShoppingCart(int saleItemId)
        {
            var selectedItem = _context.SaleItem.FirstOrDefault(p => p.SaleItemId == saleItemId);

            if(selectedItem!=null)
            {
                _shoppingCart.AddToCart(selectedItem, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int saleItemId)
        {
            var selectedItem = _context.SaleItem.FirstOrDefault(p => p.SaleItemId == saleItemId);

            if(selectedItem!=null)
            {
                _shoppingCart.RemoveFromCart(selectedItem);
            }   
            return RedirectToAction("Index");
        }
        // // GET: ShoppingCarts/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var shoppingCart = await _context.ShoppingCart
        //         .SingleOrDefaultAsync(m => m.ShoppingCartId == id);
        //     if (shoppingCart == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     return View(shoppingCart);
        // }

        // // GET: ShoppingCarts/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // // POST: ShoppingCarts/Create
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("ShoppingCartId")] ShoppingCart shoppingCart)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(shoppingCart);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(shoppingCart);
        // }

        // // GET: ShoppingCarts/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var shoppingCart = await _context.ShoppingCart.SingleOrDefaultAsync(m => m.ShoppingCartId == id);
        //     if (shoppingCart == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }
        //     return View(shoppingCart);
        // }

        // // POST: ShoppingCarts/Edit/5
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("ShoppingCartId")] ShoppingCart shoppingCart)
        // {
        //     if (id != shoppingCart.ShoppingCartId)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(shoppingCart);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!ShoppingCartExists(shoppingCart.ShoppingCartId))
        //             {
        //               Response.StatusCode = 404;
        //               return View("NotFound");
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(shoppingCart);
        // }

        // // GET: ShoppingCarts/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var shoppingCart = await _context.ShoppingCart
        //         .SingleOrDefaultAsync(m => m.ShoppingCartId == id);
        //     if (shoppingCart == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     return View(shoppingCart);
        // }

        // // POST: ShoppingCarts/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var shoppingCart = await _context.ShoppingCart.SingleOrDefaultAsync(m => m.ShoppingCartId == id);
        //     _context.ShoppingCart.Remove(shoppingCart);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool ShoppingCartExists(int id)
        // {
        //     return _context.ShoppingCart.Any(e => e.ShoppingCartId == id);
        // }
    }
}
