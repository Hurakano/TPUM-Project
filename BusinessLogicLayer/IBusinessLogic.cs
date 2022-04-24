using System;
using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public interface IBusinessLogic
    {
        void AddNewBook(BookDTO book);
        void RemoveBookById(Guid id);
        BookDTO GetBookById(Guid id);
        BookDTO GetBookByTitle(string title);
        List<BookDTO> GetAllBooks();
        List<BookDTO> GetAvailableBooks();
        bool IsBookLoaned(Guid bookId);

        void AddNewReader(ReaderDTO reader);
        void RemoveReaderById(Guid id);
        ReaderDTO GetReaderById(Guid id);
        ReaderDTO GetReaderByName(string name);
        List<ReaderDTO> GetAllReaders();

        void LoanBook(Guid readerId, Guid bookId, DateTime now, DateTime returnDate);
        void RemoveLoanById(Guid id);
        void ReturnBook(Guid bookId);
        LoanDTO GetLoanById(Guid id);
        List<LoanDTO> GetAllLoans();
        List<LoanDTO> GetAllLoansByReader(Guid readerId);
        LoanDTO GetLoanByBook(Guid bookId);
        List<LoanDTO> GetLoansBetweenDates(DateTime begin, DateTime end);
        List<LoanDTO> GetOverdueLoans(DateTime currentDate);

        IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer);
    }
}
