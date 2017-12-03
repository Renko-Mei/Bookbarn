using System;
using System.Collections;
using System.Collections.Generic;
using BookBarn.Data;

namespace BookBarn.Models
{
  public class Order
  {
    private readonly InitialModelsContext _context;
    private readonly ShoppingCart _shoppingCart;
    public Order(InitialModelsContext context, ShoppingCart shoppingCart)
    {
        _context = context;
        _shoppingCart = shoppingCart;
    }

    public void CreateOrder(Order order){
      //var shoppingCartItems = _shoppingCart.ShoppingCartItems;
      order.OrderDate = DateTime.Now;
      var shoppingCartItems =  _shoppingCart.ShoppingCartItems;

      foreach(var item in shoppingCartItems)
      {
        order.SaleItems.Add(item.SaleItem);    
      }

      _context.Order.Add(order);
      _context.SaveChanges();
    }
    public int OrderId { get; set; }
    public float SalePrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public bool IsSold { get; set; }

    public List<SaleItem> SaleItems { get; set; }
    public String BuyerId { get; set; }
    public String SellerId { get; set; }
  }
}

//dotnet aspnet-codegenerator controller -name OrdersController -m Order -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
