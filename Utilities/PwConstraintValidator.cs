﻿using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBarn.Utilities
{
    public class PwConstraintValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            if (password.Length < 8)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Password too short, should have at least 8 characters"
                }));
            }
            if (!password.Any(c => char.IsUpper(c)))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordDoesNotContainUpperCase",
                    Description = "Password should contain at least one uppercase letter (A-Z)."
                }));
            }
            if (!password.Any(c => char.IsDigit(c)))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordDoesNotContainDigit",
                    Description = "Password should contain at least one number (0-9)."
                }));
            }
            if (password.ToLowerInvariant().Contains(user.UserName.ToLowerInvariant()))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password cannot contain your user name."
                }));
            }
            if (new string(password.Distinct().ToArray()).Length < 6)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordInsufficientUniqueChar",
                    Description = "Password should have at least 6 unique characters."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
