using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using BookBarn.Data;
using BookBarn.Models.CheckoutViewModel;

namespace BookBarn.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InitialModelsContext _context;
        private readonly ShoppingCart _shoppingCart;
        private readonly AuthenticationContext _aContext;
        //private Order _order;
        public OrdersController(InitialModelsContext context, ShoppingCart shoppingCart, AuthenticationContext aContext)
        {
            _aContext = aContext;
            _context = context;
            _shoppingCart = shoppingCart;
            //_order = order;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Address address)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if(_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, please add some books first");
            }
            if(ModelState.IsValid)
            {   
                var name = _aContext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Id;
                // The address model has to save the above name
                address.UserKey = name;

                var shoppingCartItems =  _shoppingCart.ShoppingCartItems;
                 // For each seller in the shopping cart, create new order 
                var sellers = shoppingCartItems.GroupBy(x => x.SaleItem.UserKey);
                foreach(var seller in sellers)
                {
                    string sellerId = seller.Key;
                    
                    var newOrder = new Order()
                    {
                        OrderDate = DateTime.Now,
                        IsSold = true,
                        BuyerId = name,
                        SellerId = sellerId,
                        SaleItems = new List<SaleItem>(),
                        SalePrice = 0
                    };

                    foreach(var saleItems in seller)
                    {
                        var newSaleItem = saleItems.SaleItem;
                        if(newSaleItem != null)
                        {   
                            newOrder.SalePrice += newSaleItem.Price;
                            newOrder.SaleItems.Add(newSaleItem);
                        }
                    }
                    _context.Order.Add(newOrder);
                }
                _context.Address.Add(address);
                _context.SaveChanges();
                return RedirectToAction("CheckoutComplete");
            }
            return View();
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutComplete = "Thank you for ordering from BookBarn!";
            return View();
        }
    }
}
