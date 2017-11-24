using System;
using System.Collections;
using System.Collections.Generic;

namespace BookBarn.Models
{
  public class ShoppingCart
  {
    public int ShoppingCartId { get; set; }
    // public List<SaleItem> SaleItems { get; set; }
  }
}


//dotnet aspnet-codegenerator controller -name ShoppingCartsController -m ShoppingCart -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
