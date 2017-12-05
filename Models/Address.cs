using System;
using BookBarn.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBarn.Models
{
  public class Address
  {
    public int AddressId { get; set; }

    [Required]
    [Display(Name = "Legal Name")]
    public string LegalName {get; set;}

    [Required]
    public string StreetAddress { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    [Display(Name = "Province/Territory")]
    public string Province { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; }

    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    public string UserKey { get; set; }
  }
}


















//using System;

// namespace BookBarn.Models
// {
//   public class Address
//   {
//     public int AddressId { get; set; }
//     public string Unit { get; set; }
//     public string StreetNumber { get; set; }
//     public string StreetName { get; set; }
//     public string PostalCode { get; set; }
//   }
// }

// dotnet aspnet-codegenerator controller -name AddressController -m Address -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
