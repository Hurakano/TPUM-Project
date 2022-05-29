using System;
using System.Collections.Generic;

namespace LibraryServer.BusinessLogicLayer
{
    public interface IBusinessLogic
    {
        void AddNewBook(BookDTO book);
        bool RemoveBookById(Guid id);
        BookDTO GetBookById(Guid id);
        BookDTO GetBookByTitle(string title);
        List<BookDTO> GetAllBooks();
        List<BookDTO> GetAvailableBooks();
        bool IsBookLoaned(Guid bookId);

        void AddNewReader(ReaderDTO reader);
        bool RemoveReaderById(Guid id);
        ReaderDTO GetReaderById(Guid id);
        ReaderDTO GetReaderByName(string name);
        List<ReaderDTO> GetAllReaders();

        bool LoanBook(Guid readerId, Guid bookId, DateTime now, DateTime returnDate);
        bool RemoveLoanById(Guid id);
        bool ReturnBook(Guid bookId);
        LoanDTO GetLoanById(Guid id);
        List<LoanDTO> GetAllLoans();
        List<LoanDTO> GetAllLoansByReader(Guid readerId);
        LoanDTO GetLoanByBook(Guid bookId);
        List<LoanDTO> GetLoansBetweenDates(DateTime begin, DateTime end);
        List<LoanDTO> GetOverdueLoans(DateTime currentDate);

        IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer);
        IDisposable SubscribeToDataUpdatedEvent(IObserver<DataTypeUpdated> dataType);
    }

    public enum DataTypeUpdated
    {
        Readers, Books, Loans
    }

    public static class LibraryLogicFactory
    {
        public static IBusinessLogic Create(int repositorySelect)
        {
            ApplicationDataLayer.ILibraryData data;

            switch (repositorySelect)
            {
                default:
                case 0:
                    data = ApplicationDataLayer.LibraryDataFactory.CreateDataLayer();
                    break;
                case 1:
                    data = ApplicationDataLayer.LibraryDataFactory.CreateDataLayer();
                    data.FillData(new ApplicationDataLayer.ExampleFiller());
                    break;
            }

            return new LibraryLogic(data);
        }
    }
}
