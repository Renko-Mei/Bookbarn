using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using final_project.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//for UseFileServer. May delete later
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

//for ChatRoom
using final_project.ChatRoom.ClientSide;
using final_project.ChatRoom.ServerSide;

//using WebSocketASPNetCore.WebSocketManager;



namespace final_project
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
            services.AddWebSocketManager();
            services.AddMvc();
            services.AddTransient<ClientHandler>();

            
            services.AddDbContext<final_projectContext>(options =>
              options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
           
          
            
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



            app.UseWebSockets();
            //app.UseMiddleware<ServerMiddleware>();
           
            app.UseStaticFiles();

            //Mark - use static page
            //我调用websockets
            

    //然后，通过分配"/LiveChat"路径来branch pipeline,如果调用的路径和"/LiveChat"一样就运行branch (middleware)
    //在extensions里面，通过使用IApplicationBuilder来公开需要使用的middleware的位置, 
            

            //app.MapWebSocketManager("/LiveChat", serviceProvider.GetService<ChartHandler>());

            //  app.UseFileServer(new FileServerOptions(){
            //      FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"ClientChat")),
            //      RequestPath = new PathString("/ClientChat"),
            //      EnableDirectoryBrowsing = true
            //  });






            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // routes.MapSpaFallbackRoute(
                //     name: "spa-fallback",
                //     defaults: new { controller = "Home", action = "Index" });
            });
             app.MapWebSocketManager("/LiveChat", serviceProvider.GetService<ClientHandler>());
        }
    }
}
