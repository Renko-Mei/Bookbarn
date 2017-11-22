using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBarn.Utilities
{
    public class PwInNameValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {

            if (password.ToLowerInvariant().Contains(user.UserName.ToLowerInvariant()))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password cannot contain your user name."
                }));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
