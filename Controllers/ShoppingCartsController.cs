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
            if (User.Identity.IsAuthenticated){
                 var items = _shoppingCart.GetShoppingCartItems();
                _shoppingCart.ShoppingCartItems = items;
                var shoppingCartView = new ShoppingCartViewModel
                {
                    ShoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
                };
                return View(shoppingCartView);
                //return View(await _context.ShoppingCart.ToListAsync());
            }else{
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }   
           
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
    }
}
