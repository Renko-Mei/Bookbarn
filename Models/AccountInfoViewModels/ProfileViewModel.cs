using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models.AccountInfoViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public bool EmailConfirmation { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}