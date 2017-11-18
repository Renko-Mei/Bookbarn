using Microsoft.AspNetCore.Identity;


namespace BookBarn.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
