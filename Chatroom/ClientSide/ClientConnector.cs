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


        public ClientConnector(ServerHandler handler) : base(handler)
        {

        }
        public string UserName { get; set; }
        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            //check if receiver name is in the list, if yes, offer to receiver 
            var receiver = HandlerCollect.Connections.FirstOrDefault(m => ((ClientConnector)m).UserName == receiveMessage.Receiver); 
            if (receiver == null)
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                { 
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
                await receiver.SendMessageAsync(sendMessage);
            }
        }




        private class ReceiveMessage
        { 
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