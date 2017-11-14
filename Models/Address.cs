using System;

namespace final_project.Models
{
  public class Address
  {
    public int ID { get; set; }
    public string Unit { get; set; }
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string PostalCode { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name AddressController -m Address -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
