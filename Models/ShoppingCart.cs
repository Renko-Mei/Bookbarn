using System;
using System.Collections;
using System.Collections.Generic;
using BookBarn.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookBarn.Models
{
  public class ShoppingCart
  {
    private readonly InitialModelsContext _context;

    private ShoppingCart(InitialModelsContext context)
    {
      _context = context;
    }

    public static ShoppingCart GetCart(IServiceProvider services)
    {
      ISession  session = services.GetRequiredService<IHttpContextAccessor>()? // get access to our session
        .HttpContext.Session;
      var context = services.GetService<InitialModelsContext>();
      string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString(); // check to see if we already have a cart

      session.SetString("CartId", cartId);
      return new ShoppingCart(context) {ShoppingCartId = cartId};
    }

    public void AddToCart(SaleItem item, int amount)
    {
      var shoppingCartItem =
        _context.ShoppingCartItems.SingleOrDefault(
          s => s.SaleItem.SaleItemId == item.SaleItemId &&  s.ShoppingCartTemp == ShoppingCartId
        );
        
        if (shoppingCartItem == null )
        {
          shoppingCartItem = new ShoppingCartItem
          {
            ShoppingCartTemp = ShoppingCartId,
            SaleItem = item,
            Amount = 1
          };
          _context.ShoppingCartItems.Add(shoppingCartItem);
        }
        else
        {
          shoppingCartItem.Amount ++;
        }
        _context.SaveChanges();
    }

    public int RemoveFromCart(SaleItem item)
    {
      var shoppingCartItem = 
        _context.ShoppingCartItems.SingleOrDefault(
          s => s.SaleItem.SaleItemId == item.SaleItemId && s.ShoppingCartTemp == ShoppingCartId
        );
      var localAmount = 0;
      if(shoppingCartItem != null)
      {
        if(shoppingCartItem.Amount > 1)
        {
          shoppingCartItem.Amount --;
          localAmount = shoppingCartItem.Amount;
        }
        else
        {
          _context.ShoppingCartItems.Remove(shoppingCartItem);
        }
      }
      _context.SaveChanges();
      return localAmount;
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
      return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(c => c.ShoppingCartTemp == ShoppingCartId)
                                  .Include(x => x.SaleItem)
                                  .ToList());
      
    }

    public void ClearCart()
    {
      var cartItems = _context.ShoppingCartItems.Where(cart => cart.ShoppingCartTemp == ShoppingCartId);

      _context.ShoppingCartItems.RemoveRange(cartItems);
      _context.SaveChanges();
    }

    public decimal GetShoppingCartTotal()
    {
      var total = _context.ShoppingCartItems.Where(c => c.ShoppingCartTemp == ShoppingCartId).Select( c => c.SaleItem.Price * c.Amount).Sum();

      return (decimal)total;
    }

    public string ShoppingCartId { get; set; }
    public List<ShoppingCartItem> ShoppingCartItems{get; set; }
  }
}


//dotnet aspnet-codegenerator controller -name ShoppingCartsController -m ShoppingCart -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
