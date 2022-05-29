using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace LibraryClient.LibraryClientData
{
    public class ClientData : IClientData
    {
        public event EventHandler<bool> OnResult;
        public event EventHandler<int> OnDataUpdated;
        public event EventHandler<List<Loan>> OnLoanOverdue;

        private WebClient WebSocket;
        public bool SocketConnected
        {
            get
            {
                if (WebSocket != null)
                    return WebSocket.SocketState == WebSocketState.Open;
                else
                    return false;
            }
        }

        private List<Book> Books = new List<Book>();
        private List<Reader> Readers = new List<Reader>();
        private List<Loan> Loans = new List<Loan>();

        private object DataLock = new object();

        public ClientData(IClientWebSocket socketAdapter)
        {
            Task.Factory.StartNew(() => WebClient.Connect("ws://localhost:8080/", OnConnect, socketAdapter));
        }

        private void OnConnect(WebClient _webClient)
        {
            WebSocket = _webClient;
            WebSocket.OnMessage = HandleMessage;
            _ = WebSocket.SendAsync("GetBooks");
            _ = WebSocket.SendAsync("GetReaders");
            _ = WebSocket.SendAsync("GetLoans");
            _ = WebSocket.SendAsync("SubscribeEvent");
        }

        public Book GetBookById(Guid id)
        {
            Book book = null;

            lock (DataLock)
            {
                book = Books.Find(x => x.Id == id);
            }

            return book;
        }

        public Reader GetReaderById(Guid id)
        {
            Reader reader = null;

            lock(DataLock)
            {
                reader = Readers.Find(x => x.Id == id);
            }

            return reader;
        }

        public void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset)
        {
            string message = "BorrowBook\n";
            message += bookId.ToString() + "\n";
            message += readerId.ToString() + "\n";
            message += timeOffset.ToString() + "\n";

            _ = WebSocket.SendAsync(message);
        }

        public List<Book> GetAvailableBooks()
        {
            List<Book> books = new List<Book>();

            lock(DataLock)
            {
                foreach (Book book in Books)
                {
                    if (Loans.Find(x => x.BookId == book.Id) == null)
                        books.Add(book);
                }
            }

            return books;
        }

        public Loan GetLoanById(Guid loanId)
        {
            Loan loan = null;

            lock(DataLock)
            {
                loan = Loans.Find(x => x.Id == loanId);
            }

            return loan;
        }

        public List<Loan> GetLoansByReader(Guid readerId)
        {
            List<Loan> loans = new List<Loan>();

            lock(DataLock)
            {
                loans = Loans.FindAll(x => x.ReaderId == readerId);
            }

            return loans;
        }

        public List<Reader> GetReaders()
        {
            List<Reader> readers;

            lock(DataLock)
            {
                readers = new List<Reader>(Readers);
            }

            return readers;
        }

        public void ReturnBook(Guid bookId)
        {
            string message = "ReturnBook\n";
            message += bookId.ToString();

            _ = WebSocket.SendAsync(message);
        }

        public void HandleMessage(string message)
        {
            StringReader reader = new StringReader(message);
            string command = reader.ReadLine();

            switch(command)
            {
                case "Books":
                    lock (DataLock)
                    {
                        Books = ParseBooks(reader);
                    }
                    OnDataUpdated?.Invoke(this, 0);
                    break;
                case "Readers":
                    lock (DataLock)
                    {
                        Readers = ParseReaders(reader);
                    }
                    OnDataUpdated?.Invoke(this, 1);
                    break;
                case "Loans":
                    lock (DataLock)
                    {
                        Loans = ParseLoans(reader);
                    }
                    OnDataUpdated?.Invoke(this, 2);
                    break;
                case "Result":
                    bool result = reader.ReadLine() == "true"? true: false;
                    OnResult?.Invoke(this, result);
                    break;
                case "OverdueEvent":
                    List<Loan> loans = ParseLoans(reader);
                    OnLoanOverdue?.Invoke(this, loans);
                    break;
            }
        }

        private List<Book> ParseBooks(StringReader reader)
        {
            List<Book> books = new List<Book>();
            int count = int.Parse(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Guid id = Guid.Parse(reader.ReadLine());
                string title = reader.ReadLine();
                string author = reader.ReadLine();

                books.Add(new Book { Id = id, Title = title, Author = author });
            }

            return books;
        }

        private List<Reader> ParseReaders(StringReader reader)
        {
            List<Reader> readers = new List<Reader>();
            int count = int.Parse(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Guid id = Guid.Parse(reader.ReadLine());
                string name = reader.ReadLine();

                readers.Add(new Reader { Id = id, Name = name });
            }

            return readers;
        }

        private List<Loan> ParseLoans(StringReader reader)
        {
            List<Loan> loans = new List<Loan>();
            int count = int.Parse(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Guid id = Guid.Parse(reader.ReadLine());
                Guid bookId = Guid.Parse(reader.ReadLine());
                Guid readerId = Guid.Parse(reader.ReadLine());
                DateTime borrowDate = DateTime.Parse(reader.ReadLine());
                DateTime returnDate = DateTime.Parse(reader.ReadLine());

                loans.Add(new Loan { Id = id, BookId = bookId, ReaderId = readerId, BorrowDate = borrowDate, ReturnDate = returnDate});
            }

            return loans;
        }
    }
}
