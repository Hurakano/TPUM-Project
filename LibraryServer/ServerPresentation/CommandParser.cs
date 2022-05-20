using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LibraryServer.BusinessLogicLayer;

namespace LibraryServer.ServerPresentation
{
    class CommandHandler: IObserver<List<LoanDTO>>, IDisposable
    {
        public static IBusinessLogic LibraryLogic { get; set; }
        private static List<CommandHandler> SocketHandlers = new List<CommandHandler>();

        private WebSocketConnection SocketConnection;
        private IDisposable EventUnsubscriber = null;

        public static void OnConnectionSocket(WebSocketConnection webSocket)
        {
            Console.WriteLine("Socket connected");
            CommandHandler handler = new CommandHandler();
            handler.SocketConnection = webSocket;
            webSocket.OnMessage = handler.OnMessageSocket;
            webSocket.OnClose = handler.OnCloseSocket;
            webSocket.OnError = handler.OnErrorSocket;
            SocketHandlers.Add(handler);
        }
        private static void SocketClosed(CommandHandler _handler)
        {
            SocketHandlers.Remove(_handler);
        }

        public void OnMessageSocket(string message)
        {
            Console.WriteLine(message);
            StringReader reader = new StringReader(message);
            string command = reader.ReadLine();

            switch(command)
            {
                case "GetBooks":
                    GetBooks();
                    break;
                case "GetBookTitle":
                    string title = reader.ReadLine();
                    GetBookByTitle(title);
                    break;
                case "SubscribeEvent":
                    if(EventUnsubscriber == null)
                    {
                        EventUnsubscriber = LibraryLogic.SubscribeToOverdueEvent(this);
                    }
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    return;
            }
        }

        public void OnCloseSocket()
        {
            Console.WriteLine("Socket closed");
            CommandHandler.SocketClosed(this);
        }

        public void OnErrorSocket()
        {
            Console.WriteLine("Socket error");
            CommandHandler.SocketClosed(this);
        }

        private void GetBooks()
        {
            List<BookDTO> books = LibraryLogic.GetAllBooks();
            string message = books.Count.ToString() + "\n";
            foreach(BookDTO book in books)
            {
                message += book.Title + "\n";
            }

            _ = SocketConnection.SendAsync(message);
        }

        private void GetBookByTitle(string title)
        {
            BookDTO book = LibraryLogic.GetBookByTitle(title);
            string message = "";
            if (book != null)
            {
                message += book.Author + "\n";
            }
            else
            {
                message += "null\n";
            }

            _ = SocketConnection.SendAsync(message);
        }

        public void OnCompleted()
        {
            EventUnsubscriber?.Dispose();
            EventUnsubscriber = null;
        }

        public void OnError(Exception error)
        {
            EventUnsubscriber?.Dispose();
            EventUnsubscriber = null;
        }

        public void OnNext(List<LoanDTO> value)
        {
            string message = "OverdueEvent\n";
            message += value.Count.ToString() + "\n";

            foreach(LoanDTO loan in value)
            {
                message += loan.Id.ToString() + "\n";
                message += loan.BookId.ToString() + "\n";
                message += loan.ReaderId.ToString() + "\n";
                message += loan.ReturnDate.ToString() + "\n";
            }

            _ = SocketConnection.SendAsync(message);
        }

        public void Dispose()
        {
            EventUnsubscriber?.Dispose();
        }
    }
}
