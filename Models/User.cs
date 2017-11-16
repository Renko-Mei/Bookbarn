using Microsoft.AspNetCore.Identity;


namespace final_project.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
