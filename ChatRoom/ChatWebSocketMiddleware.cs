using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace BookBarn.ChatRoom
{
    public class ChatWebSocketMiddleware
    {
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public static List<BookBarn.ChatRoom.ChatData> historicalMessage = new List<BookBarn.ChatRoom.ChatData>();
        private readonly RequestDelegate _next;

        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                CancellationToken ct = context.RequestAborted;
                WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[4096];
                string userName = context.Request.Query["Name"].ToString();
                //var socketId = Guid.NewGuid().ToString();
                var chatData = new ChatData(){info=userName + " has entered into the chat room."};
                _sockets.TryAdd(userName, currentSocket);
                foreach (var a in _sockets)
                {
                    if(a.Value.State == WebSocketState.Open)
                    {
                        await SendStringAsync(a.Value, chatData, ct);

                    }
                } 

                while (true)
                {
                    var income = await currentSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if(income.MessageType ==WebSocketMessageType.Close)
                    {
                         _sockets.TryRemove(userName, out currentSocket);
                        chatData = new ChatData(){info=userName + " has left the chat room."};

                        foreach (var b in _sockets)
                        {
                            if(b.Value.State == WebSocketState.Open)
                            {
                                await SendStringAsync(b.Value, chatData, ct);
                                
                            }
                        }
                        await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                        currentSocket.Dispose();
                        break; 
                    }

                   // var response = await ReceiveStringAsync(currentSocket, ct);//
                    var chatDataStr = await ArraySegmentToStringAsync(new ArraySegment<byte>(buffer, 0, income.Count));
                    if(string.IsNullOrEmpty(chatDataStr))
                    {
                        if(currentSocket.State == WebSocketState.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        chatData = JsonConvert.DeserializeObject<ChatData>(chatDataStr); 
                        //await SendStringAsync(_sockets.Where(t => t != userName).ToList(), chatData);                                              
                        foreach (var socket in _sockets)
                        {
                            if(socket.Value.State == WebSocketState.Open)
                            {
                                await SendStringAsync(socket.Value, chatData, ct);
                            }
                        } 
                    }
                    
                }

                WebSocket dummy;
                _sockets.TryRemove(userName, out dummy);
                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                currentSocket.Dispose();
                
            }
            else
            {
                await _next.Invoke(context);
                return;
            }
            
        }




        private static Task SendStringAsync(WebSocket socket, ChatData data, CancellationToken ct = default(CancellationToken))
        {
            var chatData = JsonConvert.SerializeObject(data);
            var buffer = Encoding.UTF8.GetBytes(chatData);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        // private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        // {
        //     var buffer = new ArraySegment<byte>(new byte[8192]);
        //     using (var ms = new MemoryStream())
        //     {
        //         WebSocketReceiveResult result;
        //         do
        //         {
        //             ct.ThrowIfCancellationRequested();

        //             result = await socket.ReceiveAsync(buffer, ct);
        //             ms.Write(buffer.Array, buffer.Offset, result.Count);
        //         }
        //         while (!result.EndOfMessage);

        //         ms.Seek(0, SeekOrigin.Begin);
        //         if (result.MessageType != WebSocketMessageType.Text)
        //         {
        //             return null;
        //         }

        //         using (var reader = new StreamReader(ms, Encoding.UTF8))
        //         {
        //             return await reader.ReadToEndAsync();
        //         }
        //     }
        // }

        static async Task<string> ArraySegmentToStringAsync(ArraySegment<byte> arraySegment)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
