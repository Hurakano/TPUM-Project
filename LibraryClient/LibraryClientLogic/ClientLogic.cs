using System;
using System.Collections.Generic;
using System.Threading;
using LibraryClient.LibraryClientData;

namespace LibraryClient.LibraryClientLogic
{
    public class ClientLogic : IClientLogic, IObservable<List<LoanDTO>>
    {
        public event EventHandler<int> OnDataUpdated;
        public event EventHandler<bool> OnTransactionResult;
        private readonly List<IObserver<List<LoanDTO>>> OverdueEventObservers = new List<IObserver<List<LoanDTO>>>();
        private ReaderWriterLockSlim DataAccessLock = new ReaderWriterLockSlim();

        private IClientData LibraryData;

        public ClientLogic(IClientData dataLayer)
        {
            LibraryData = dataLayer;
            LibraryData.OnResult += (sender, arg) => OnTransactionResult?.Invoke(sender, arg);
            LibraryData.OnDataUpdated += DataUpdatedHandler;
            LibraryData.OnLoanOverdue += HandleOverdueEvent;
        }

        public void BorrowBook(Guid bookId, Guid readerId, DateTime returnDate)
        {
            DataAccessLock.EnterWriteLock();
            try
            {
                if (LibraryData.GetAvailableBooks().Find(x => x.Id == bookId) != null)
                    LibraryData.BorrowBook(readerId, bookId, returnDate);
            }
            finally
            {
                DataAccessLock.ExitWriteLock();
            }
        }

        public List<LoanDTO> GetAllLoansByReader(Guid readerId)
        {
            List<LoanDTO> loans = new List<LoanDTO>();

            DataAccessLock.EnterReadLock();
            try
            {
                foreach (Loan loan in LibraryData.GetLoansByReader(readerId))
                {
                    loans.Add(new LoanDTOImpl { Id = loan.Id, BookId = loan.BookId, ReaderId = loan.ReaderId, BorrowDate = loan.BorrowDate, ReturnDate = loan.ReturnDate });
                }
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return loans;
        }

        public List<ReaderDTO> GetAllReaders()
        {
            List<ReaderDTO> readers = new List<ReaderDTO>();

            DataAccessLock.EnterReadLock();
            try
            {
                foreach (Reader reader in LibraryData.GetReaders())
                {
                    readers.Add(new ReaderDTOImpl { Id = reader.Id, Name = reader.Name });
                }
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return readers;
        }

        public List<BookDTO> GetAvailableBooks()
        {
            List<BookDTO> books = new List<BookDTO>();

            DataAccessLock.EnterReadLock();
            try
            {
                foreach (Book book in LibraryData.GetAvailableBooks())
                {
                    books.Add(new BookDTOImpl { Id = book.Id, Title = book.Title, Author = book.Author });
                }
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return books;
        }

        public BookDTO GetBookById(Guid id)
        {
            Book book;
            BookDTO returnBook = null;

            DataAccessLock.EnterReadLock();
            try
            {
                book = LibraryData.GetBookById(id);
                if (book != null)
                    returnBook = new BookDTOImpl { Id = book.Id, Title = book.Title, Author = book.Author };
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return returnBook;
        }

        public LoanDTO GetLoanById(Guid id)
        {
            LoanDTO returnLoan = null;

            DataAccessLock.EnterReadLock();
            try
            {
                Loan loan = LibraryData.GetLoanById(id);
                if (loan != null)
                    returnLoan = new LoanDTOImpl { Id = loan.Id, BookId = loan.BookId, ReaderId = loan.ReaderId, BorrowDate = loan.BorrowDate, ReturnDate = loan.ReturnDate };
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return returnLoan;
        }

        public ReaderDTO GetReaderById(Guid id)
        {
            ReaderDTO returnReader = null;

            DataAccessLock.EnterReadLock();
            try
            {
                Reader reader = LibraryData.GetReaderById(id);
                if (reader != null)
                    returnReader = new ReaderDTOImpl { Id = reader.Id, Name = reader.Name };
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            return returnReader;
        }

        public void ReturnBook(Guid bookId)
        {
            DataAccessLock.EnterWriteLock();
            try
            {
                LibraryData.ReturnBook(bookId);
            }
            finally
            {
                DataAccessLock.ExitWriteLock();
            }
        }

        private void DataUpdatedHandler(object sender, int arg)
        {
            OnDataUpdated?.Invoke(this, arg);
        }

        private void HandleOverdueEvent(object sender, List<Loan> loans)
        {
            List<LoanDTO> loansForward = new List<LoanDTO>();
            foreach (Loan loan in loans)
            {
                loansForward.Add(new LoanDTOImpl { Id = loan.Id, BookId = loan.BookId, ReaderId = loan.ReaderId, BorrowDate = loan.BorrowDate, ReturnDate = loan.ReturnDate });
            }

            List<IObserver<List<LoanDTO>>> observers;
            DataAccessLock.EnterReadLock();
            try
            {
                observers = new List<IObserver<List<LoanDTO>>>(OverdueEventObservers);
            }
            finally
            {
                DataAccessLock.ExitReadLock();
            }

            foreach(IObserver<List<LoanDTO>> observer in observers)
            {
                observer.OnNext(loansForward);
            }
        }

        public IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer)
        {
            return Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<List<LoanDTO>> observer)
        {
            DataAccessLock.EnterWriteLock();
            try
            {
                OverdueEventObservers.Add(observer);
            }
            finally
            {
                DataAccessLock.ExitWriteLock();
            }

            return new SubscriptionCanceller(OverdueEventObservers, observer, DataAccessLock);
        }

        class SubscriptionCanceller: IDisposable
        {
            private List<IObserver<List<LoanDTO>>> ObserversList;
            private IObserver<List<LoanDTO>> Observer;
            private ReaderWriterLockSlim ListLock;

            public SubscriptionCanceller(List<IObserver<List<LoanDTO>>> _observersList, IObserver<List<LoanDTO>> _observer, ReaderWriterLockSlim _lock)
            {
                ObserversList = _observersList;
                Observer = _observer;
                ListLock = _lock;
            }
            public void Dispose()
            {
                ListLock.EnterWriteLock();
                try
                {
                    ObserversList.Remove(Observer);
                }
                finally
                {
                    ListLock.ExitWriteLock();
                }
            }
        }
    }
}
