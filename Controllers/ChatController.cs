using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBarn.Data;
using BookBarn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BookBarn.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public ChatController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            ILogger<AccountInfoController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }




        // GET: /<controller>/
  
        // public async Task <IActionResult> Index()
        // {
        //     var user = await userManager.GetUserAsync(User);
        //     if(user ==null){

        
        //     }
        // }
    }
            
 }
