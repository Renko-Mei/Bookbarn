using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Bookbarn.ChatRoom.ServerSide
{
    public abstract class ServerConnection
    {
        public ServerHandler HandlerCollect { get; } //websockethandler.cs -->A1

        public WebSocket WebSocketCollect { get; set; }//send and receive data after the WebSocket upgrade has completed.

        public ServerConnection(ServerHandler handler)//-->A2
        {
            HandlerCollect = handler;//用上面的 public WebSocketHandler Handler { get; }来把客户端的handler存到Handler上
        }

        public virtual async Task SendMessageAsync(string message)
        {
            if (WebSocketCollect.State != WebSocketState.Open) return;//这里的websocket是我们上面的那个{get; set;}
            var arr = Encoding.UTF8.GetBytes(message);

            var buffer = new ArraySegment<byte>(//
                    array: arr,//Gets the original array containing the range of elements that the array segment delimits
                    offset: 0,//Gets the position of the first element in the range delimited by the array segment, relative to the start of the original array
                    count: arr.Length);//Gets the number of elements in the range delimited by the array segment

            await WebSocketCollect.SendAsync(//Sends data over the WebSocket connection asynchronously. 这里的websocket是我们上面的那个{get; set;}
                buffer: buffer,//The buffer to be sent over the connection.
                messageType: WebSocketMessageType.Text,//Indicates whether the application is sending a binary or text message.
                endOfMessage: true,//Indicates whether the data in “buffer” is the last part of a message.
                cancellationToken: CancellationToken.None//The token that propagates the notification that operations should be canceled.
                );//
        }

        public abstract Task ReceiveAsync(string message);//
    }
}
