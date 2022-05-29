using System;
using System.IO;
using System.Collections.Generic;
using LibraryServer.BusinessLogicLayer;

namespace LibraryServer.ServerPresentation
{
    class CommandHandler : IObserver<List<LoanDTO>>, IObserver<DataTypeUpdated>, IDisposable
    {
        private static IBusinessLogic m_logic = null;
        public static IBusinessLogic LibraryLogic
        {   
            get { return m_logic; }
            set { if (m_logic == null)m_logic = value; }
        }
        private static List<CommandHandler> SocketHandlers = new List<CommandHandler>();

        private WebSocketConnection SocketConnection;
        private IDisposable EventUnsubscriber = null;
        private IDisposable DataUpdatedUnsubscriber = null;

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

            Guid tmpId;

            switch(command)
            {
                case "GetBooks":
                    GetBooks();
                    break;
                case "GetBookByTitle":
                    string title = reader.ReadLine();
                    GetBookByTitle(title);
                    break;
                case "GetReaders":
                    GetReaders();
                    break;
                case "GetLoans":
                    GetLoans();
                    break;
                case "GetLoansByReader":
                    tmpId = Guid.Parse(reader.ReadLine());
                    GetLoansByReader(tmpId);
                    break;
                case "GetLoanById":
                    tmpId = Guid.Parse(reader.ReadLine());
                    GetLoanById(tmpId);
                    break;
                case "GetAvailableBooks":
                    GetAvailableBooks();
                    break;
                case "ReturnBook":
                    tmpId = Guid.Parse(reader.ReadLine());
                    ReturnBook(tmpId);
                    break;
                case "BorrowBook":
                    tmpId = Guid.Parse(reader.ReadLine());
                    Guid id2 = Guid.Parse(reader.ReadLine());
                    DateTime returnDate = DateTime.Parse(reader.ReadLine());
                    BorrowBook(tmpId, id2, returnDate);
                    break;
                case "SubscribeEvent":
                    if(EventUnsubscriber == null)
                    {
                        EventUnsubscriber = LibraryLogic.SubscribeToOverdueEvent(this);
                        DataUpdatedUnsubscriber = m_logic.SubscribeToDataUpdatedEvent(this);
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
            string message = "Books\n" + books.Count.ToString() + "\n";
            foreach(BookDTO book in books)
            {
                message += BookToString(book);
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void GetBookByTitle(string title)
        {
            BookDTO book = LibraryLogic.GetBookByTitle(title);
            string message = "Book\n";
            if (book != null)
            {
                message += BookToString(book);
            }
            else
            {
                message += "null\n";
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void GetReaders()
        {
            List<ReaderDTO> readers = LibraryLogic.GetAllReaders();
            string message = "Readers\n" + readers.Count.ToString() + "\n";
            foreach(ReaderDTO reader in readers)
            {
                message += ReaderToString(reader);
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void GetLoans()
        {
            List<LoanDTO> loans = LibraryLogic.GetAllLoans();
            string message = "Loans\n" + loans.Count.ToString() + "\n";
            foreach(LoanDTO loan in loans)
            {
                message += LoanToString(loan);
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void GetLoansByReader(Guid id)
        {
            List<LoanDTO> loans = LibraryLogic.GetAllLoansByReader(id);
            string message = "Loan\n" + loans.Count.ToString() + "\n";
            foreach(LoanDTO loan in loans)
            {
                message += LoanToString(loan);
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void GetLoanById(Guid id)
        {
            LoanDTO loan = LibraryLogic.GetLoanById(id);
            string message = "Loan\n";
            if (loan != null)
                message += LoanToString(loan) + "\n";
            else
                message += "null\n";

            _ = SocketConnection.SendAsync(message);
        }
        private void GetAvailableBooks()
        {
            List<BookDTO> books = LibraryLogic.GetAvailableBooks();
            string message = "AvailableBooks\n" + books.Count.ToString() + "\n";
            foreach(BookDTO book in books)
            {
                message += BookToString(book);
            }

            _ = SocketConnection.SendAsync(message);
        }
        private void ReturnBook(Guid id)
        {
            bool result = LibraryLogic.ReturnBook(id);
            string message = "Result\n" + (result ? "true\n" : "false\n");
            _ = SocketConnection.SendAsync(message);
        }
        private void BorrowBook(Guid bookId, Guid readerId, DateTime returnDate)
        {
            DateTime borrowDate = DateTime.Now;
            bool result = LibraryLogic.LoanBook(readerId, bookId, borrowDate, returnDate);
            string message = "Result\n" + (result ? "true\n" : "false\n");
            _ = SocketConnection.SendAsync(message);
        }
        private static string BookToString(BookDTO book)
        {
            return book.Id.ToString() + "\n" + book.Title + "\n" + book.Author + "\n";
        }
        private static string ReaderToString(ReaderDTO reader)
        {
            return reader.Id.ToString() + "\n" + reader.Name + "\n";
        }
        private static string LoanToString(LoanDTO loan)
        {
            return loan.Id.ToString() + "\n" + loan.BookId.ToString() + "\n" + loan.ReaderId.ToString() + "\n" +
                   loan.BorrowDate.ToString() + "\n" + loan.ReturnDate.ToString() + "\n";
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
                message += LoanToString(loan);
            }

            _ = SocketConnection.SendAsync(message);
        }

        public void OnNext(DataTypeUpdated value)
        {
            switch(value)
            {
            case DataTypeUpdated.Books:
                GetBooks();
                break;
            case DataTypeUpdated.Readers:
                GetReaders();
                break;
            case DataTypeUpdated.Loans:
                GetLoans();
                break;
            }
        }

        public void Dispose()
        {
            EventUnsubscriber?.Dispose();
            DataUpdatedUnsubscriber?.Dispose();
        }

    }
}
