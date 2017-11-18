using System;

namespace BookBarn.Models
{
  public class Address
  {
    public int AddressId { get; set; }
    public string Unit { get; set; }
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string PostalCode { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name AddressController -m Address -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
