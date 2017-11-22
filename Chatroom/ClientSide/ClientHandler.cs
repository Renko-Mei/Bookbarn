using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BookBarn.ChatRoom.ServerSide;




namespace BookBarn.ChatRoom.ClientSide
{
    public class ClientHandler : ServerHandler
    { 
        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<ServerConnection> OnConnected(HttpContext context)
        {
            var SenderName = context.Request.Query["Name"];
            var connection = Connections.FirstOrDefault(m => ((ClientConnector)m).UserName == SenderName);//check if the user already exists in the list, if not, add to the list
            //var AvailableConnects = Connections.SelectMany(Available=>((ClientConnector)Available).UserName);

            if (connection == null)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync(); //Transitions the request to a WebSocket connection. Returns: A task representing the completion of the transition.

                connection = new ClientConnector(this)
                { //connection includes NickName and WebSocket
                    UserName = SenderName, 
                    WebSocketCollect = webSocket 
                };

                Connections.Add(connection);
            }

            return connection;
        }
    }
}
