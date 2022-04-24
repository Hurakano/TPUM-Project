using System;
using System.Collections.Generic;
using BusinessLogicLayer;

namespace PresentationLayer.LibraryModel
{
    public class LibraryModel
    {
        private IBusinessLogic Library;
        public OverdueReporter OverdueWatcher;
        private IDisposable ObserverStopper;

        public LibraryModel()
        {
            Library = LibraryLogic.Create(1);
            OverdueWatcher = new OverdueReporter();
            ObserverStopper = Library.SubscribeToOverdueEvent(OverdueWatcher);
        }

        ~LibraryModel()
        {
            ObserverStopper.Dispose();
        }

        public List<ReaderDTO> GetReaders()
        {
            return Library.GetAllReaders();
        }

        public ReaderDTO GetReaderByName(string name)
        {
            return Library.GetReaderByName(name);
        }

        public List<LoanPresenter> GetLoansByReader(Guid readerId)
        {
            List<LoanPresenter> presentLoans = new List<LoanPresenter>();
            foreach (LoanDTO loan in Library.GetAllLoansByReader(readerId))
            {
                presentLoans.Add(new LoanPresenter(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(readerId).Name, loan));
            }

            return presentLoans;
        }

        public LoanPresenter GetPresentedLoan(LoanDTO loan)
        {
            return new LoanPresenter(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(loan.ReaderId).Name, loan);
        }

        public List<BookDTO> GetAvailableBooks()
        {
            return Library.GetAvailableBooks();
        }

        public void ReturnBook(Guid bookId)
        {
            Library.ReturnBook(bookId);
        }

        public void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset)
        {
            DateTime time = DateTime.Now;
            Library.LoanBook(readerId, bookId, time, timeOffset);
        }
    }
}
