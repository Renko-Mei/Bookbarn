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
        //public static object objLock = new object();
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private static ConcurrentDictionary<string, string> _nameCompare = new ConcurrentDictionary<string, string>();
        public static List<string> historicalMessage = new List<string>();
        private readonly RequestDelegate _next;
        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context){
            int TotalUser = 0;
            string tempUserName = "unknown";
            
            if (context.WebSockets.IsWebSocketRequest){
                
                CancellationToken ct = context.RequestAborted;
                WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
                var socketId = Guid.NewGuid().ToString();
                var addSuccess = _sockets.TryAdd(socketId, currentSocket);
                TotalUser +=1;
                
                // if(addSuccess){
                //     var currentUser = _sockets.Count;
                //     var available =  "[SYSTEM]: Currently, " + currentUser +" people in this chatroom";
                //     await SendStringAsync( _sockets, available);
                // }

                while (true){
                    if (ct.IsCancellationRequested){
                        break;
                    }
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var receive = await currentSocket.ReceiveAsync(buffer, ct);
                    // if(receive.MessageType == WebSocketMessageType.Close){
                    //     var leftSuccess = _sockets.TryRemove(socketId, out currentSocket);
                    //     TotalUser -=1;
                    //     if(leftSuccess){
                    //         foreach(var t in _nameCompare){
                    //             if(t.Key == socketId){
                    //                 tempUserName = t.Value;  
                    //             }
                    //         }
                    //         var leftMessage = "[SYSTEM]: " + tempUserName +" left the chatroom. Now remains " + TotalUser + " people";
                    //         await SendStringAsync( _sockets, leftMessage);
                    //         break;
                                    
                    //     } 
                    // }


                    var response = await ReceiveStringAsync(currentSocket, CancellationToken.None);

                    if(response.Contains("[SYSTEM]: Welcome")){
                        var WelcomeIndex = response.LastIndexOf("Welcome");
                        int tempS = WelcomeIndex +8;
                        int tempL = response.Length-tempS;
                        var userName = response.Substring(tempS, tempL);
                        var isCompared = _nameCompare.TryAdd(socketId, userName);
                        if(isCompared){
                            //var currentUser = _sockets.Count;
                            response = response + " Currently " + TotalUser + " people online";
                            //var available =  "[SYSTEM]: Currently, " + TotalUser.ToString() +" people in this chatroom";
                            //await SendStringAsync( _sockets, available);
                        } 
                    }

                    if(string.IsNullOrEmpty(response)){
                        if(currentSocket.State != WebSocketState.Open){
                            break;
                        }
                    }
                    else{
                        await SendStringAsync(_sockets, response);
                    }
                        
                    
                    
                }

               // WebSocket dummy;
                _sockets.TryRemove(socketId, out currentSocket);
                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                currentSocket.Dispose();
                
            }else{
                await _next.Invoke(context);
                return;
            } 
        }

        private async static Task SendStringAsync( ConcurrentDictionary<string, WebSocket> socket, string data){
            
            SaveHistoricalMessage(data);
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            foreach (var a in _sockets){
                if(a.Value.State == WebSocketState.Open){
                    await a.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);   
                }
            }
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken)){
            
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream()){
                WebSocketReceiveResult result;
                do{
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text){
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8)){
                    return await reader.ReadToEndAsync();
                }
            }
        }

        //static object lockSaveMsg = new object();
        public static void SaveHistoricalMessage(string data){
            historicalMessage.Add(data);   
        }
    }
}





//         private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
//         public static List<BookBarn.ChatRoom.ChatData> historicalMessage = new List<BookBarn.ChatRoom.ChatData>();
//         private readonly RequestDelegate _next;

//         public ChatWebSocketMiddleware(RequestDelegate next)
//         {
//             _next = next;
//         }

//         public async Task Invoke(HttpContext context)
//         {
//             if (context.WebSockets.IsWebSocketRequest)
//             {
//                 CancellationToken ct = context.RequestAborted;
//                 WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
//                 var buffer = new byte[4096];
//                 string userName = context.Request.Query["Name"].ToString();
//                 //var socketId = Guid.NewGuid().ToString();
//                 var chatData = new ChatData(){info=userName + " has entered into the chat room."};
//                 _sockets.TryAdd(userName, currentSocket);
//                 foreach (var a in _sockets)
//                 {
//                     if(a.Value.State == WebSocketState.Open)
//                     {
//                         await SendStringAsync(a.Value, chatData, ct);

//                     }
//                 } 

//                 while (true)
//                 {
//                     var income = await currentSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//                     if(income.MessageType ==WebSocketMessageType.Close)
//                     {
//                          _sockets.TryRemove(userName, out currentSocket);
//                         chatData = new ChatData(){info=userName + " has left the chat room."};

//                         foreach (var b in _sockets)
//                         {
//                             if(b.Value.State == WebSocketState.Open)
//                             {
//                                 await SendStringAsync(b.Value, chatData, ct);
                                
//                             }
//                         }
//                         await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
//                         currentSocket.Dispose();
//                         break; 
//                     }

//                    // var response = await ReceiveStringAsync(currentSocket, ct);//
//                     var chatDataStr = await ArraySegmentToStringAsync(new ArraySegment<byte>(buffer, 0, income.Count));
//                     if(string.IsNullOrEmpty(chatDataStr))
//                     {
//                         if(currentSocket.State == WebSocketState.Closed)
//                         {
//                             break;
//                         }
//                     }
//                     else
//                     {
//                         chatData = JsonConvert.DeserializeObject<ChatData>(chatDataStr); 
//                         //await SendStringAsync(_sockets.Where(t => t != userName).ToList(), chatData);                                              
//                         foreach (var socket in _sockets)
//                         {
//                             if(socket.Value.State == WebSocketState.Open)
//                             {
//                                 await SendStringAsync(socket.Value, chatData, ct);
//                             }
//                         } 
//                     }
                    
//                 }

//                 WebSocket dummy;
//                 _sockets.TryRemove(userName, out dummy);
//                 await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
//                 currentSocket.Dispose();
                
//             }
//             else
//             {
//                 await _next.Invoke(context);
//                 return;
//             }
            
//         }




//         private static Task SendStringAsync(WebSocket socket, ChatData data, CancellationToken ct = default(CancellationToken))
//         {
//             SaveHistoricalMessg(data);
//             var chatData = JsonConvert.SerializeObject(data);
//             var buffer = Encoding.UTF8.GetBytes(chatData);
//             var segment = new ArraySegment<byte>(buffer);
//             return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
//         }

//         // private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
//         // {
//         //     var buffer = new ArraySegment<byte>(new byte[8192]);
//         //     using (var ms = new MemoryStream())
//         //     {
//         //         WebSocketReceiveResult result;
//         //         do
//         //         {
//         //             ct.ThrowIfCancellationRequested();

//         //             result = await socket.ReceiveAsync(buffer, ct);
//         //             ms.Write(buffer.Array, buffer.Offset, result.Count);
//         //         }
//         //         while (!result.EndOfMessage);

//         //         ms.Seek(0, SeekOrigin.Begin);
//         //         if (result.MessageType != WebSocketMessageType.Text)
//         //         {
//         //             return null;
//         //         }

//         //         using (var reader = new StreamReader(ms, Encoding.UTF8))
//         //         {
//         //             return await reader.ReadToEndAsync();
//         //         }
//         //     }
//         // }

//         static async Task<string> ArraySegmentToStringAsync(ArraySegment<byte> arraySegment)
//         {
//             using (var ms = new MemoryStream())
//             {
//                 ms.Write(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
//                 ms.Seek(0, SeekOrigin.Begin);
//                 using (var reader = new StreamReader(ms, Encoding.UTF8))
//                 {
//                     return await reader.ReadToEndAsync();
//                 }
//             }
//         }

//         public static void SaveHistoricalMessg(ChatData data)
//         {
//             var size = 40;
//             historicalMessage.Add(data);
//             if (historicalMessage.Count >= size)
//             {

//                 historicalMessage.RemoveRange(0, 30);
//             }
//         }
//     }
// }
