using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.WebSockets;

namespace LibraryClient.LibraryClientData
{
    class WebClient: WebSocketConnection
    {
        public static async Task Connect(string address, Action<WebClient> onConnect, IClientWebSocket socketAdapter)
        {
            Uri uri = new Uri(address);
            IClientWebSocket clientSocket = socketAdapter;
            await clientSocket.ConnectAsync(uri, CancellationToken.None);
            if (clientSocket.State == WebSocketState.Open)
            {
                onConnect?.Invoke(new WebClient(clientSocket));
            }
            else
            {
                throw new Exception("Connecting socket failed");
            }
        }

        private IClientWebSocket Socket;
        public WebSocketState SocketState { get => Socket.State; }

        private WebClient(IClientWebSocket _socket)
        {
            Socket = _socket;
            Task.Factory.StartNew(() => ClientMessageLoop());
        }

        ~WebClient()
        {
            if(Socket != null)
                Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
        }

        protected override Task SendTask(string message)
        {
            ArraySegment<Byte> arraySeg = new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message));
            return Socket.SendAsync(arraySeg, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public override Task DisconnectAsync()
        {
            return Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
        }

        private void ClientMessageLoop()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result = Socket.ReceiveAsync(segment, CancellationToken.None).Result;

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
                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = Socket.ReceiveAsync(segment, CancellationToken.None).Result;
                    count += result.Count;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, count);
                OnMessage?.Invoke(message);
            }
        }
    }
}
