using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using final_project.ChatRoom.ServerSide;




namespace final_project.ChatRoom.ClientSide
{
    public class ClientHandler : ServerHandler
    { //WebSocketHandler作为基础，影响BufferSize, OnConnected, Connections
        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<ServerConnection> OnConnected(HttpContext context)
        {
            var SenderName = context.Request.Query["Name"]; //name是sender
            var connection = Connections.FirstOrDefault(m => ((ClientConnector)m).UserName == SenderName);//检测这个user是不是已经存在于Connections这个list里，如果没有，就加list 

            if (connection == null)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync(); //Transitions the request to a WebSocket connection. Returns: A task representing the completion of the transition.

                connection = new ClientConnector(this)
                { //connection里包括了NickName和WebSocket
                    UserName = SenderName, //存到ChartConnection里面的NickName{get set}
                    WebSocketCollect = webSocket //这个是存到WebSocketConnection里的WebSocket {},能连到WebSocketConnection是因为ChartConnetction本来就从WebSocketConnection那里来的
                };

                Connections.Add(connection);//加到WebSocketHandler的Connections List里面
            }

            return connection;//return connection给Startup里的MapWebSocketManger来看路径对不对
        }
    }
}
//这个就是测试user有没有上线，但是现在必须要进入聊天页面才能知道，要改成login之后就默认在线
