using System;
using System.Collections.Generic;
using LibraryClient.LibraryClientLogic;

namespace PresentationLayer.LibraryModel
{
    class LibraryModel: AbstractLibraryModel
    {
        private IClientLogic Library;

        public LibraryModel()
        {
            Library = ClienLogicFactory.Create();
            OverdueWatcher = new OverdueReporter();
            ObserverStopper = Library.SubscribeToOverdueEvent(OverdueWatcher);
            Library.OnDataUpdated += DataUpdatedHandler;
            Library.OnTransactionResult += HandleTransactionResult;
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
                readers.Add(new ReaderPresenterImpl(reader.Id, reader.Name));
            }

            return readers;
        }

        public override List<LoanPresenter> GetLoansByReader(Guid readerId)
        {
            List<LoanPresenter> presentLoans = new List<LoanPresenter>();
            foreach (LoanDTO loan in Library.GetAllLoansByReader(readerId))
            {
                presentLoans.Add(new LoanPresenterImpl(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(readerId).Name, loan.BookId, loan.ReaderId, loan.ReturnDate));
            }

            return presentLoans;
        }

        public override LoanPresenter GetLoanById(Guid loanId)
        {
            LoanDTO loan = Library.GetLoanById(loanId);
            return new LoanPresenterImpl(Library.GetBookById(loan.BookId).Title, Library.GetReaderById(loan.ReaderId).Name, loan.BookId, loan.ReaderId, loan.ReturnDate);
        }

        public override List<BookPresenter> GetAvailableBooks()
        {
            List<BookPresenter> books = new List<BookPresenter>();
            foreach(BookDTO book in Library.GetAvailableBooks())
            {
                books.Add(new BookPresenterImpl(book.Id, book.Title, book.Author));
            }

            return books;
        }

        public override void ReturnBook(Guid bookId)
        {
            Library.ReturnBook(bookId);
        }

        public override void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset)
        {
            Library.BorrowBook(bookId, readerId, timeOffset);
        }
    }
}
