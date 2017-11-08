using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace final_project
{
    public static class Extension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();

            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(Handler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, 
                                                              PathString path,
                                                              Handler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<Middleware>(handler));
        }
    }
}