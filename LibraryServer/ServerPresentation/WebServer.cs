using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.WebSockets;

namespace LibraryServer.ServerPresentation
{
    static class WebServer
    {
        public static async Task Server(int port, Action<WebSocketConnection> onConnection)
        {
            Uri uri = new Uri($"http://localhost:{port}/");
            await ServerLoop(uri, onConnection);
        }

        public static async Task ServerLoop(Uri uri, Action<WebSocketConnection> onConnection)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add(uri.ToString());
            server.Start();
            while (true)
            {
                HttpListenerContext context = await server.GetContextAsync();
                if(!context.Request.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                    continue;
                }
                HttpListenerWebSocketContext socketContext = await context.AcceptWebSocketAsync(null);
                WebSocketConnection socketConnection = new ServerWebSocketConnection(socketContext.WebSocket);
                onConnection?.Invoke(socketConnection);
            }
        }
    }

    class ServerWebSocketConnection : WebSocketConnection
    {
        private WebSocket Socket = null;

        public ServerWebSocketConnection(WebSocket _socket)
        {
            Socket = _socket;
            Task.Factory.StartNew(() => ServerMessageLoop(Socket));
        }

        protected override Task SendTask(string message)
        {
            Console.WriteLine("Message: " + message);
            ArraySegment<Byte> arraySeg = new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message));
            return Socket.SendAsync(arraySeg, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public override Task DisconnectAsync()
        {
            return Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutting down socket", CancellationToken.None);
        }

        private void ServerMessageLoop(WebSocket socket)
        {
            byte[] buffer = new byte[1024];

            while(true)
            {
                ArraySegment<byte> arraySegments = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result = Socket.ReceiveAsync(arraySegments, CancellationToken.None).Result;

                if(result.MessageType == WebSocketMessageType.Close)
                {
                    OnClose?.Invoke();
                    Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
                    return;
                }

                int count = result.Count;
                while (!result.EndOfMessage)
                {
                    if (count >= buffer.Length)
                    {
                        OnClose?.Invoke();
                        Socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Message too long", CancellationToken.None);
                        return;
                    }
                    arraySegments = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = Socket.ReceiveAsync(arraySegments, CancellationToken.None).Result;
                    count += result.Count;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, count);
                OnMessage?.Invoke(message);
            }
        }
    }
}
