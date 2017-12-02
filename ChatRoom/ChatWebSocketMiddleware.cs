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
        private static List<string> duplicate = new List<string>();
        public static List<string> historicalMessage = new List<string>();
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private static ConcurrentDictionary<string, string> _nameCompare = new ConcurrentDictionary<string, string>();
        private readonly RequestDelegate _next;

        
        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        int TotalUser = 0;

        public async Task Invoke(HttpContext context)
        {
            
            string tempUserName = "Guest";
            
            if (context.WebSockets.IsWebSocketRequest)
            {
                
                CancellationToken ct = context.RequestAborted;
                WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
                var socketId = Guid.NewGuid().ToString();
                var addSuccess = _sockets.TryAdd(socketId, currentSocket);
                TotalUser +=1;
                int check = 0;

                while (true)
                {
                    try
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }

                        foreach(var t in _nameCompare)
                        {
                            if(t.Key == socketId)
                            {
                                tempUserName = t.Value;  
                            }       
                        }

                        var response = await ReceiveStringAsync(currentSocket,TotalUser, tempUserName,ct);

                        if(response.Contains("[SYSTEM]: Welcome"))
                        {
                            var WelcomeIndex = response.LastIndexOf("Welcome");
                            int tempS = WelcomeIndex +8;
                            int tempL = response.Length-tempS-1;
                            var userName = response.Substring(tempS, tempL);
                            // foreach(var m in _nameCompare){
                            //         if(m.Value ==userName){
                            //             TotalUser -=1;
                            //             break;
                            //         }
                            //     }    
                            var isCompared = _nameCompare.TryAdd(socketId, userName);

                            if(isCompared)
                            {   
                                response = response + " Currently " + TotalUser + " people online";
                            } 
                        }

                        if(string.IsNullOrEmpty(response))
                        {
                            if(currentSocket.State != WebSocketState.Open)
                            {
                                break;
                            }
                        }
                        else
                        {
                            await SendStringAsync(_sockets, response);
                        } 
                    }
                    catch(Exception ex)
                    {
                        if(check ==0)
                        {
                            TotalUser -=1;
                            //var afterLeft = TotalUser-1;
                            var CloseWindow = "[SYSTEM]: " + tempUserName +" left the chatroom. Now remains " + TotalUser + " people";
                        
                            await SendStringAsync(_sockets, CloseWindow);
                            check =1;
                        }
                        
                    }
                    
                }

               // WebSocket dummy;
                _sockets.TryRemove(socketId, out currentSocket);
                if(check ==0)
                {
                    await SendStringAsync(_sockets, "test");
                }
                
                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                currentSocket.Dispose();
                
            }
            else
            {
                await _next.Invoke(context);
                return;
                //context.Response.StatusCode = 404;
            } 
        }

        private async static Task SendStringAsync( ConcurrentDictionary<string, WebSocket> socket, string data)
        {
            
            SaveHistoricalMessage(data);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var a in _sockets){
                if(a.Value.State == WebSocketState.Open)
                {
                    await a.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);   
                }
            }
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, int TotalUser, string tempUserName, CancellationToken ct = default(CancellationToken))
        {
            
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();
                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
        public static void SaveHistoricalMessage(string data)
        {
            historicalMessage.Add(data);   
        }
    }
}


