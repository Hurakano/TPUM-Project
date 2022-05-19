using System;
using System.Collections.Generic;
using LibraryServer.BusinessLogicLayer;

namespace PresentationLayer.LibraryModel
{
    class LibraryModel: AbstractLibraryModel
    {
        private IBusinessLogic Library;

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

        public override List<ReaderPresenter> GetReaders()
        {
            List<ReaderPresenter> readers = new List<ReaderPresenter>();

            foreach(ReaderDTO reader in Library.GetAllReaders())
            {
                readers.Add(new ReaderPresenter(reader.Id, reader.Name));
            }

            return readers;
        }

        public override List<LoanPresenter> GetLoansByReader(Guid readerId)
        {
            List<LoanPresenter> presentLoans = new List<LoanPresenter>();
            foreach (LoanDTO loan in Library.GetAllLoansByReader(readerId))
            {
                presentLoans.Add(new LoanPresenter(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(readerId).Name, loan.BookId, loan.ReaderId, loan.ReturnDate));
            }

            return presentLoans;
        }

        public override LoanPresenter GetLoanById(Guid loanId)
        {
            LoanDTO loan = Library.GetLoanById(loanId);
            return new LoanPresenter(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(loan.ReaderId).Name, loan.BookId, loan.ReaderId, loan.ReturnDate);
        }

        public override List<BookPresenter> GetAvailableBooks()
        {
            List<BookPresenter> books = new List<BookPresenter>();
            foreach(BookDTO book in Library.GetAvailableBooks())
            {
                books.Add(new BookPresenter(book.Id, book.Title, book.Author));
            }

            return books;
        }

        public override void ReturnBook(Guid bookId)
        {
            Library.ReturnBook(bookId);
        }

        public override void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset)
        {
            DateTime time = DateTime.Now;
            Library.LoanBook(readerId, bookId, time, timeOffset);
        }
    }
}
