using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models.AccountInfoViewModels
{
    public class ChangeAddressViewModel
    {
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Legal Name")]
        public string LegalName {get; set;}

        [Required]
        public string Address { get; set; }

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