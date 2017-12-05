using System;
using System.Collections;
using System.Collections.Generic;

namespace BookBarn.Models
{
  public class ShoppingCartItem
  {
    public int ShoppingCartItemId { get; set; }
    public SaleItem SaleItem {get; set; }
    public int Amount {get; set; }
    public string ShoppingCartTemp {get; set; }
  }
}

