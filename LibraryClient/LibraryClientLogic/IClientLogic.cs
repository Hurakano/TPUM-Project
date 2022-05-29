using System;
using System.Collections.Generic;
using LibraryClient.LibraryClientData;

namespace LibraryClient.LibraryClientLogic
{
    public interface IClientLogic
    {
        public event EventHandler<int> OnDataUpdated;
        public event EventHandler<bool> OnTransactionResult;

        public BookDTO GetBookById(Guid id);
        public ReaderDTO GetReaderById(Guid id);
        public List<ReaderDTO> GetAllReaders();
        public List<LoanDTO> GetAllLoansByReader(Guid readerId);
        public LoanDTO GetLoanById(Guid id);
        public List<BookDTO> GetAvailableBooks();
        public void ReturnBook(Guid bookId);
        public void BorrowBook(Guid bookId, Guid readerId, DateTime returnDate);

        public IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer);
    }

    public static class ClienLogicFactory
    {
        public static IClientLogic Create()
        {
            IClientData data = ClientDataFactory.GetClient();
            return new ClientLogic(data);
        }
    }
}
