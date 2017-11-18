// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using System.Net.WebSockets;
// using System.Collections.Concurrent;



// namespace BookBarn.ChatRoom
// {
// 	public class CustomedMiddleware
// 	{
		
// 		protected int BufferSize {get => 4096;}//?
// 		public async Task OnConnected(HttpContext context)
// 		{
// 			var SenderName = context.Request.Query["Name"];
// 			//var connection = _connections;
			
// 		}

// 	}

// 	public class Data
// 	{
// 		public string UserName {get; set;}
// 		private static ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>(); 
// 	}

// }