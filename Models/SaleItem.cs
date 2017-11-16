using System;

namespace final_project.Models
{
  public class SaleItem
  {
    public int SaleItemId { get; set; }
    public float Price { get; set; }
    public string Quality { get; set; }
    public bool IsSold { get; set; }

    public int BookId { get; set; }
  }
}

//dotnet aspnet-codegenerator controller -name SaleItemsController -m SaleItem -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries