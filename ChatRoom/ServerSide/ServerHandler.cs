using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;





namespace Bookbarn.ChatRoom.ServerSide
{
    public abstract class ServerHandler
    {
        protected abstract int BufferSize { get; }

        private List<ServerConnection> _connections = new List<ServerConnection>();//-->A3 //这个list以后也可以改成dictionary

        public List<ServerConnection> Connections { get => _connections; }//A4 这个和上面那个以及handler里的websocketconnection可以放在一个文件里

        public async Task ListenConnection(ServerConnection connection)
        {
            var buffer = new byte[BufferSize];

            while (connection.WebSocketCollect.State == WebSocketState.Open)
            {
                var result = await connection.WebSocketCollect.ReceiveAsync( //Receives data from the WebSocket connection asynchronously.这里的websocket是connection.cs里的
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count); //.Count : Indicates the number of bytes that the WebSocket received

                    await connection.ReceiveAsync(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await OnDisconnected(connection);
                }
            }
        }

        public virtual async Task OnDisconnected(ServerConnection connection)//这个要来干嘛？
        {
            if (connection != null)
            {
                _connections.Remove(connection);

                await connection.WebSocketCollect.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Closed by the WebSocketHandler",
                    cancellationToken: CancellationToken.None);
            }
        }

        public abstract Task<ServerConnection> OnConnected(HttpContext context);
    }
}
