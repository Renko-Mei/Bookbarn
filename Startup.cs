using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using BookBarn.Data;

//for UseFileServer. May delete later
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

//for ChatRoom

using UserManagement.Utilities;
using BookBarn.Utilities;


//for confirmation email
using BookBarn.Services;
using BookBarn.Services.Paypal;

namespace BookBarn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //for confirmation email
            services.AddTransient<IEmailSender, EmailSender>();

            //for the shopping cart 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShoppingCart.GetCart(sp)); // object associated with the control

            services.AddMvc();
            services.AddMemoryCache(); //for the shopping cart 
            services.AddSession(); //for the shopping cart 
            // Configure database model
            services
                .AddDbContext<InitialModelsContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services
                .AddDbContext<AuthenticationContext>(options => options
                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));


            // Configure Authentication. now need to confirm the reality of email address first.
            services.AddIdentity<User, IdentityRole>(config =>{config.SignIn.RequireConfirmedEmail = true;})
                .AddEntityFrameworkStores<AuthenticationContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            // Configure identity constraints
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // Register custom username / password combination validator
            services.AddTransient<IPasswordValidator<User>, PwConstraintValidator>();

            // Register a custom user email validator
            services.AddTransient<IUserValidator<User>, UserEmailValidator>();

            // Configure cookie
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = Configuration["Globals:AppName"];
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(90);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            services.Configure<PaypalSettings>(Configuration.GetSection("PaypalSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
              ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            


            app.UseWebSockets();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseSession(); // shopping cart 
            app.UseMiddleware<BookBarn.ChatRoom.ChatWebSocketMiddleware>();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            //     routes.MapSpaFallbackRoute(
            //         name: "spa-fallback",
            //         defaults: new { controller = "Home", action = "Index" });
             });
        }
    }
}
