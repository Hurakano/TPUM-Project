using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace LibraryClient.LibraryClientData
{
    public interface IClientData
    {
        public event EventHandler<bool> OnResult;
        public event EventHandler<int> OnDataUpdated;
        public event EventHandler<List<Loan>> OnLoanOverdue;

        public bool SocketConnected { get; }
        public Task Dissconnect();

        public Book GetBookById(Guid id);
        public Reader GetReaderById(Guid id);
        public List<Reader> GetReaders();
        public List<Loan> GetLoansByReader(Guid readerId);
        public Loan GetLoanById(Guid loanId);
        public List<Book> GetAvailableBooks();
        public void ReturnBook(Guid bookId);
        public void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset);
    }

    public static class ClientDataFactory
    {
        public static IClientData GetClient()
        {
            return new ClientData(new ClientWebSocketAdapter());
        }
    }
}
