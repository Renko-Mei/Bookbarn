using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; //Assembly
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder; //iApplicationBuilder
using Microsoft.AspNetCore.Http;    //PathString
using Microsoft.Extensions.DependencyInjection; //IserviceCollection





namespace final_project.ChatRoom.ServerSide
{
	public static class ServerExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, ServerHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<ServerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            var handlerBaseType = typeof(ServerHandler);

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == handlerBaseType)
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}
