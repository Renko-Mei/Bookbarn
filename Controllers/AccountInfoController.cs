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
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using BookBarn.Services;
using BookBarn.Data;
using System.Text;
using System.Text.Encodings.Web;
using BookBarn.Models.AccountInfoViewModels;
using ChartJSCore.Models;




namespace BookBarn.Controllers
{
    public class AccountInfoController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;
        private readonly InitialModelsContext context;
        private readonly AuthenticationContext Acontext;

        public AccountInfoController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            ILogger<AccountInfoController> logger,
            InitialModelsContext context,
            AuthenticationContext Acontext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.context = context;
            this.Acontext = Acontext;
        }

        public string UserID()
        {
            return Acontext.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Id;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null){
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }
            
            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmation = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            
            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null){
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            
            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";
            return RedirectToAction(nameof(ChangePassword));
        }


        [HttpGet]
        public async Task<IActionResult> ChangeAddress()
        {
            var user = await userManager.GetUserAsync(User);
            var temp = await context.Address.ToListAsync();
            var viewList = from a in temp 
                        where a.UserKey == UserID() 
                        select a;
           
            if(viewList.Count() !=0)
            {
                var _AddressId = viewList.FirstOrDefault(c => c.UserKey == UserID()).AddressId;
                var _LegalName = viewList.FirstOrDefault(c => c.UserKey == UserID()).LegalName;
                var _StreetAddress = viewList.FirstOrDefault(c => c.UserKey == UserID()).StreetAddress;
                var _City = viewList.FirstOrDefault(c => c.UserKey == UserID()).City;
                var _Province = viewList.FirstOrDefault(c => c.UserKey == UserID()).Province;
                var _Country = viewList.FirstOrDefault(c => c.UserKey == UserID()).Country;
                var _PostalCode = viewList.FirstOrDefault(c => c.UserKey == UserID()).PostalCode;
                var _PhoneNumber = viewList.FirstOrDefault(c => c.UserKey == UserID()).PhoneNumber;
                var _UserKey = viewList.FirstOrDefault(c => c.UserKey == UserID()).UserKey;
                if(user ==null){
                    Response.StatusCode = 401;
                    return View("NotLoggedIn");
                }

                var model = new ChangeAddressViewModel{
                    AddressId = _AddressId,
                    LegalName = _LegalName,
                    StreetAddress = _StreetAddress,
                    City = _City,
                    Province = _Province,
                    Country = _Country,
                    PostalCode = _PostalCode,
                    PhoneNumber = _PhoneNumber,
                    UserKey = _UserKey
                };

                return View(model);

            }
            else
            {
                return View("NoAvailableAddress");
            }
             
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAddress(ChangeAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.AddressId = context.Address.FirstOrDefault(c => c.UserKey == UserID()).AddressId;
                model.UserKey = UserID();
                context.Update(model);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            if (model.UserKey ==UserID())
            {
                return View(model);
            }
            else
            {
                return View("NoAccess");
            }
        }








        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
             var user = await userManager.GetUserAsync(User);
            if(user == null){
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }           
            var orders = this.context.Order
            .Where(x =>  x.BuyerId == user.Id)
            .OrderByDescending(x => x.OrderDate)
            .ToList();
                            

            //ViewData["orders"] = orders;
            return View(orders);
        }


        [HttpGet]
        public async Task<IActionResult> SalesVisualization()
        {   
            var user = await userManager.GetUserAsync(User);
            if(user == null){
                Response.StatusCode = 401;
                return View("NotLoggedIn");
            }

            Chart chart = new Chart();
            chart.Type = "line";
            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            // Here the data is generated by Orders to be deleted
            // var timeSeries = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };
            // var orders = new List<double>() { 65, 59, 80, 81, 56, 55, 40 };

            var timeSeries = new List<string>();
            var orders = new List<double>();

            var ordersData = this.context.Order.Where(x => x.SellerId == user.Id).Select(c => new {c.SalePrice, c.OrderDate}).ToList();

            foreach (var item in ordersData)
            {
                //timeSeries.Add(System.DateTime.ParseExact(item.OrderDate,"mmm-yy",null));
                timeSeries.Add(item.OrderDate.ToString("yyyy-MM-dd"));
                orders.Add((double)item.SalePrice);    
            }

            //System.Diagnostics.Debug.WriteLine(ordersData);
        

            data.Labels = timeSeries;
            LineDataset dataset = new LineDataset()
            {
                Label = "Total Revenue",
                Data = orders,
                Fill = false,
                LineTension = 0.1,
                BackgroundColor = "rgba(75, 192, 192, 0.4)",
                BorderColor = "rgba(75,192,192,1)",
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<string>() { "rgba(75,192,192,1)" },
                PointBackgroundColor = new List<string>() { "#fff" },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
                PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);
            chart.Data = data;
            ViewData["chart"] = chart;
            return View();
        }
    }
}