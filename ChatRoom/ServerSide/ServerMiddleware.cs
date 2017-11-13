using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace final_project.ChatRoom.ServerSide
{
	public class ServerMiddleware
    {
        private readonly RequestDelegate _next;//A function that can process an HTTP request. Returns: A task that represents the completion of request processing.
        private ServerHandler _webSocketHandler { get; set; }//

        public ServerMiddleware(RequestDelegate next, ServerHandler webSocketHandler) //
        {
            _next = next;//
            _webSocketHandler = webSocketHandler;//
        }

        public async Task Invoke(HttpContext context)//
        {
            if (context.WebSockets.IsWebSocketRequest)//
            {
                var connection = await _webSocketHandler.OnConnected(context);
                if (connection != null)
                {
                    await _webSocketHandler.ListenConnection(connection);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
        }
    }
}
