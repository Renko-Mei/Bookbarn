using System;
using System.Collections.Generic;
using System.Linq;

namespace BookBarn.Models.ShoppingCartViewModel
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart {get; set;}
        public decimal ShoppingCartTotal {get; set;}
    }
}
