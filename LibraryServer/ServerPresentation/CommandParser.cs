using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LibraryServer.BusinessLogicLayer;

namespace LibraryServer.ServerPresentation
{
    static class CommandParser
    {
        public static IBusinessLogic LibraryLogic { get; set; }
        private static WebSocketConnection SocketConnection;

        public static void OnConnectionSocket(WebSocketConnection webSocket)
        {
            Console.WriteLine("Socket connected");
            SocketConnection = webSocket;
            webSocket.OnMessage = CommandParser.OnMessageSocket;
            webSocket.OnClose = CommandParser.OnCloseSocket;
            webSocket.OnError = CommandParser.OnErrorSocket;
        }

        public static void OnMessageSocket(string message)
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
                default:
                    Console.WriteLine("Unknown command");
                    return;
            }
        }

        public static void OnCloseSocket()
        {
            Console.WriteLine("Socket closed");
        }

        public static void OnErrorSocket()
        {
            Console.WriteLine("Socket error");
        }

        private static void GetBooks()
        {
            List<BookDTO> books = LibraryLogic.GetAllBooks();
            string message = books.Count.ToString() + "\n";
            foreach(BookDTO book in books)
            {
                message += book.Title + "\n";
            }

            _ = SocketConnection.SendAsync(message);
        }

        private static void GetBookByTitle(string title)
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
    }
}
