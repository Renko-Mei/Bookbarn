using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//for websocket
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Logging;  
using System.Net.WebSockets;  
using System.Threading; 

//for UseFileServer. May delete later
using Microsoft.Extensions.FileProviders;
using System.IO;

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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                    //name: "spa-fallback",
                    //defaults: new { controller = "Home", action = "Index" });
            });

            //add websockets middleware
            var webSocketOptions = new WebSocketOptions()
            {
                //How frequently to send "ping" frames to the client, to ensure proxies keep the connection open.
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            //check if it's a WebSocket request and accept the WebSocket request
            app.Use(async (context, next) =>  
            {  
                if (context.Request.Path == "/ws")  
                {  
                    if (context.WebSockets.IsWebSocketRequest)  
                    {  
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();  
                        await Echo(context, webSocket);  
                    }  
                    else  
                    {  
                        context.Response.StatusCode = 400;  
                    }  
                }  
                else  
                {  
                    await next();  
                }  
            });
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"ChatroomTest")),
                RequestPath = new PathString("/ChatRoomTest"),
                EnableDirectoryBrowsing = true
            });
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)  
        {  
            var buffer = new byte[1024 * 4];  
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            while (!result.CloseStatus.HasValue)  
            {  
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);  
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            }  
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);  
        }  
    }
}
