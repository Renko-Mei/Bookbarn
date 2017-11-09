using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;



namespace final_project.ChatRoom
{
	public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        //private ConcurrentDictionary<string, List<string>> _message = new ConcurrentDictionary<string, List<string>>();

        //WebSocket allows applications to send and receive data after the WebSocket upgrade has completed
        public WebSocket GetSocketById(string id)   //名字到时候都改，该加的加，该减的减
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)//这个可能用不到
        {
            WebSocket socket;
            _sockets.TryRemove(id, out socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        //有没有可能用guid来作为每个connect的id，形成一对一的chatroom
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}