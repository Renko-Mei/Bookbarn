using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookBarn.ChatRoom.ServerSide;



namespace BookBarn.ChatRoom.ClientSide
{
    public class ClientConnector : ServerConnection
    {
        //base class 是WebSocketConnection,表明这里跑的是 WebSocketConnection(WebSocketHandler handler）{}
        //为什么是WebSocketHandler的前缀?
        public ClientConnector(ServerHandler handler) : base(handler)
        {

        }
        public string UserName { get; set; }
        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);//把JSON转换成.net可以用的type
            //这里的Handler是WebSocketHandler里的 public WebSocketHandler Handler { get; }
            //Connections是 list<WebSocketConnection> WebSocketHandler.Connections,在ClientHandler内，Connections已经被赋予NickName 和WebSocket
            //Handler.Connections 表明Connections是存在于Handler里面的的？
            //检查receiver的名字是否存在于UserList里面,如果有就赋予receiver
            var receiver = HandlerCollect.Connections.FirstOrDefault(m => ((ClientConnector)m).UserName == receiveMessage.Receiver); //Connections存在于HandlerCollect里
            //只要这个用户在线，它的名字就会被记录下来
            //这个要改，现在是用户发了东西以后说对方不在，以后，不需要，直接说不在，可以把email整合进去
            if (receiver == null)
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                { //下面的那个class
                    Sender = UserName,
                    Message = "Can not seed to " + receiveMessage.Receiver
                });
                await SendMessageAsync(sendMessage);
            }
            else
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                {
                    Sender = UserName,
                    Message = receiveMessage.Message
                });
                await receiver.SendMessageAsync(sendMessage);//调用WebSocketHandler里的SendMessageAsync(string message)
            }
        }




        private class ReceiveMessage
        { //从index里面赋予
            //public string Sender { get; set; }
            public string Receiver { get; set; }
            public string Message { get; set; }
        }
        private class SendMessage
        {
            public string Sender { get; set; }
            public string Message { get; set; }
        }
    }
}