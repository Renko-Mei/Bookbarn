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
using MailKit.Net.Smtp;
using MimeKit;

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
            if(!User.Identity.IsAuthenticated){
                Response.StatusCode = 401;
              return View("NotLoggedIn");
            }
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
                    var soldItemId ="";
                    foreach(var saleItems in seller)
                    {
                        soldItemId =" "+saleItems.SaleItem.SaleItemId.ToString()+", ";
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
                    //send email to sellers
                    var customerEmail =  _aContext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Email;
                    var customerName = _aContext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).UserName;
                    var sellerEmail =  _aContext.Users.FirstOrDefault(s => s.Id == sellerId).Email;
                    var customerLegalName = address.LegalName;
                    var customerAddress = address.StreetAddress + ", " + address.City + ", " + address.Province +", "+address.Country +", "+ address.PostalCode;
                    var customerPhoneNumber = address.PhoneNumber;
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("BookBarn", "info@bookbarncanada.com"));
                    message.To.Add(new MailboxAddress("seller", sellerEmail));
                    message.Subject = "Customer Orders";
                    var builder = new BodyBuilder();
                    builder.TextBody = @"Dear seller, you have a new order";
                    if(customerPhoneNumber ==null){
                        customerPhoneNumber = "not provided";
                    }
                    builder.HtmlBody = "<em>Customer Legal Name"+customerLegalName+"<br>Customer Email: "+customerEmail+ "<br>Customer Address:"+customerAddress+"<br>Customer Phone Number: "+ customerPhoneNumber + "</em><br><br>ItemId that customer bought: <br>"+soldItemId+"<br>";
                    message.Body = builder.ToMessageBody();
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("info@bookbarncanada.com", "InfoBookBarn");
                        client.Send(message);

                        client.Disconnect(true);
                    }
                }
                

                //remove from shopping cart
                foreach(var seller in sellers){
                    foreach(var saleItems in seller){
                        var selectedItem = _context.SaleItem.FirstOrDefault(p => p.SaleItemId == saleItems.SaleItem.SaleItemId);
                        if(selectedItem!=null)
                        {
                            _shoppingCart.RemoveFromCart(selectedItem);
                        }   
                    }
                    
                }


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
