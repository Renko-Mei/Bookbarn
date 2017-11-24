using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BookBarn.Models.IdentityViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using BookBarn.Services;
using BookBarn.Data;
using System.Text;
using System.Text.Encodings.Web;
//using BookBarn.Models.ManageViewModels;



namespace BookBarn.Controllers
{
    public class AccountInfoController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;
    }

    // public ManageController(UserManager<User> userManager,
    //        SignInManager<User> signInManager,
    //        RoleManager<IdentityRole> roleManager,
    //        IEmailSender emailSender,
    //        ILogger<UserController> logger,
    //        AuthenticationContext context)
    //     {
    //         this.userManager = userManager;
    //         this.signInManager = signInManager;
    //         this.roleManager = roleManager;
    //         this.logger = logger;
    //         this.emailSender = emailSender;
    //         this.context = context;
    //     }
}