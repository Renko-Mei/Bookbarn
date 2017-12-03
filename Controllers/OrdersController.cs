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
    public class OrdersController : Controller
    {
        private readonly InitialModelsContext _context;
        private readonly ShoppingCart _shoppingCart;

        private Order _order;
        public OrdersController(InitialModelsContext context, ShoppingCart shoppingCart, Order order)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _order = order;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if(_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, please add some books first");
            }
            if(ModelState.IsValid)
            {
                _order.CreateOrder(order);
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutComplete = "Thank you for ordering from BookBarn!";
            return View();
        }

        // // GET: Orders
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.Order.ToListAsync());
        // }

        // // GET: Orders/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var order = await _context.Order
        //         .SingleOrDefaultAsync(m => m.OrderId == id);
        //     if (order == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     return View(order);
        // }

        // // GET: Orders/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // // POST: Orders/Create
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("OrderId,SalePrice,OrderDate,ShippedDate,IsSold,BuyerId,SellerId")] Order order)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(order);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(order);
        // }

        // // GET: Orders/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
        //     if (order == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }
        //     return View(order);
        // }

        // // POST: Orders/Edit/5
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("OrderId,SalePrice,OrderDate,ShippedDate,IsSold,BuyerId,SellerId")] Order order)
        // {
        //     if (id != order.OrderId)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(order);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!OrderExists(order.OrderId))
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
        //     return View(order);
        // }

        // // GET: Orders/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     var order = await _context.Order
        //         .SingleOrDefaultAsync(m => m.OrderId == id);
        //     if (order == null)
        //     {
        //       Response.StatusCode = 404;
        //       return View("NotFound");
        //     }

        //     return View(order);
        // }

        // // POST: Orders/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
        //     _context.Order.Remove(order);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool OrderExists(int id)
        // {
        //     return _context.Order.Any(e => e.OrderId == id);
        // }
    }
}
