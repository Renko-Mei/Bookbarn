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
        string[] blockedDomains =
        {
            "@example.com"
        };

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            if (blockedDomains.Any(x => user.Email.ToLowerInvariant().EndsWith(x)))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = $"This email domain is NOT allowed."
                }));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
