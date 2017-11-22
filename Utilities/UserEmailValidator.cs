using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBarn.Utilities
{
    public class UserEmailValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            if (user.Email.ToLowerInvariant().EndsWith("@example.com"))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "example.com email addresses are NOT allowed."
                }));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
