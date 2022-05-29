using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace LibraryClient.LibraryClientData
{
    public interface IClientWebSocket
    {
        public Task ConnectAsync(Uri uri, CancellationToken token);
        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken token);
        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token);
        public Task CloseAsync(WebSocketCloseStatus closeStatus, string description, CancellationToken token);

        public WebSocketState State { get; }
    }

    public class ClientWebSocketAdapter : IClientWebSocket
    {
        private readonly ClientWebSocket Socket;

        public ClientWebSocketAdapter()
        {
            Socket = new ClientWebSocket();    
        }

        public WebSocketState State => Socket.State;

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string description, CancellationToken token)
        {
            return Socket.CloseAsync(closeStatus, description, token);
        }

        public Task ConnectAsync(Uri uri, CancellationToken token)
        {
            return Socket.ConnectAsync(uri, token);
        }

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken token)
        {
            return Socket.ReceiveAsync(buffer, token);
        }

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token)
        {
            return Socket.SendAsync(buffer, messageType, endOfMessage, token);
        }
    }
}
