using Microsoft.AspNetCore.Identity;


namespace BookBarn.Models
{
    public enum UserRole : ushort
    {
        SuperAdministrator,
        User
    }

    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
