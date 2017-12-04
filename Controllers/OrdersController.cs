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

        public string UserID()
        {
            return _aContext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Id;
        }


        public async Task<IActionResult> Checkout()
        {

            var temp = await _context.Address.ToListAsync();
            var viewList = from a in temp 
                        where a.UserKey == UserID() 
                        select a;
           
                if(viewList.Count() !=0)
                {
                    ViewData["LegalName"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).LegalName;
                    ViewData["StreetAddress"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).StreetAddress;
                    ViewData["City"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).City;
                    ViewData["Province"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).Province;
                    ViewData["Country"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).Country;
                    ViewData["PostalCode"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).PostalCode;
                    ViewData["Phone number"] = viewList.FirstOrDefault(c => c.UserKey == UserID()).PhoneNumber;
                }
                else
                {
                    ViewData["LegalName"] = "";
                    ViewData["StreetAddress"] = "";
                    ViewData["City"] = "";
                    ViewData["Province"] = "";
                    ViewData["Country"] = "";
                    ViewData["PostalCode"] = "";
                    ViewData["Phone number"] = "";
                }
                ViewData["bookNum"]= _shoppingCart.GetShoppingCartItems().Count();
            
                return View();
           
            
        }

        [HttpPost]
        public async Task <IActionResult> Checkout(Address address)
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

                //
                var tempSale = await _context.SaleItem.ToListAsync();
               

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
                        saleItems.SaleItem.IsSold = true;
                        tempSale.FirstOrDefault(d =>d.SaleItemId == saleItems.SaleItem.SaleItemId).IsSold =true;

                        var newSaleItem = saleItems.SaleItem;
                        if(newSaleItem != null)
                        {   
                            newOrder.SalePrice += newSaleItem.Price;
                            newOrder.SaleItems.Add(newSaleItem);
                        }
                    }
                    _context.Order.Add(newOrder);
                }



                //
                //var MarkShoppingCartItems =  _shoppingCart.ShoppingCartItems.




                //address
                var temp = await _context.Address.ToListAsync();

                var viewList = from a in temp 
                        where a.UserKey == UserID() 
                        select a;

                if(viewList.Count() !=0){
                    var addressId = viewList.FirstOrDefault(c => c.UserKey == UserID()).AddressId;
                    var modelAddress = await _context.Address.SingleOrDefaultAsync(m => m.AddressId == addressId);
                    modelAddress.LegalName = address.LegalName;
                    modelAddress.StreetAddress = address.StreetAddress;
                    modelAddress.City = address.City;
                    modelAddress.Province = address.Province;
                    modelAddress.Country = address.Country;
                    modelAddress.PostalCode = address.PostalCode;
                    modelAddress.PhoneNumber = address.PhoneNumber;
                    modelAddress.UserKey = UserID();
                     _context.Update(modelAddress);
                    await _context.SaveChangesAsync();
                }
                else{
                    address.UserKey = UserID();
                    _context.Add(address);
                    await _context.SaveChangesAsync();
                }

                //issolde 
                




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
