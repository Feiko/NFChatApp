using System;
using System.Text;
using System.Net.WebSockets.Server;
using System.Net;

namespace NFChatApp
{
    internal static class ChatWebSocketServer
    {
        internal static bool Started = false;
        internal static ConcurrentUserDictionary WebSocketUsers = new ConcurrentUserDictionary();
        internal static WebSocketServer WebsocketServer = new WebSocketServer(new WebSocketServerOptions()
        {
            MaxClients = 8,
            ServerName = "ChatServer",
            Port = 8080,
            Prefix = "/chat"
        });

        internal static bool Start()
        {
            if (!Started)
            {
                WebsocketServer.MessageReceived += WebsocketServer_MessageReceived;
                WebsocketServer.WebSocketClosed += WebsocketServer_WebSocketClosed;
                WebsocketServer.Start();

                Started = true;
            }

            return Started;
        }

        internal static bool AddClient(HttpListenerContext context)
        {
            string name = context.Request.Headers["Sec-WebSocket-Protocol"];
            
            if (string.IsNullOrEmpty(name)) return false;

            if (WebsocketServer == null) //todo add client  
            {
                WebSocketUsers[context.Request.RemoteEndPoint.ToString()] = name;
                return true;
            }

            return false;
        }

        private static void WebsocketServer_WebSocketClosed(object sender, WebSocketClosedEventArgs e)
        {
            WebSocketUsers.Remove(e.EndPoint.ToString());
        }

        private static void WebsocketServer_MessageReceived(object sender, System.Net.WebSockets.MessageReceivedEventArgs e)
        {
            if (e.Frame.MessageType == System.Net.WebSockets.WebSocketFrame.WebSocketMessageType.Text)
            {
               // WebsocketServer.
            }
        }
    }
}
