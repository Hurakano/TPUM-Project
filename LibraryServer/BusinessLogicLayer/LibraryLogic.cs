using System;
using System.Collections.Generic;
using LibraryServer.ApplicationDataLayer;
using System.Timers;
using System.Threading;

namespace LibraryServer.BusinessLogicLayer
{
    public class LibraryLogic: IBusinessLogic, IObservable<List<LoanDTO>>
    {
        private readonly ILibraryData DataRepository;
        private readonly List<IObserver<List<LoanDTO>>> EventObservers;
        private readonly System.Timers.Timer EventTimer;
        private readonly ReaderWriterLockSlim DataLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim OverdueObserverLock = new ReaderWriterLockSlim();

        public LibraryLogic(ILibraryData repository)
        {
            DataRepository = repository;
            EventObservers = new List<IObserver<List<LoanDTO>>>();
            EventTimer = new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds)
            {
                AutoReset = true
            };
            EventTimer.Elapsed += new ElapsedEventHandler(CheckForOverdueLoans);
            EventTimer.Start();
        }

        public void AddNewBook(BookDTO book)
        {
            DataLock.EnterWriteLock();
            try
            {
                DataRepository.AddBook(new Book(book.Title, new DateTime(), book.Author));
            }
            finally
            {
                DataLock.ExitWriteLock();
            }
        }
        public bool RemoveBookById(Guid id)
        {
            bool success = false;

            DataLock.EnterWriteLock();
            try
            {
                if (!IsBookLoanedNoLock(id))
                {
                    DataRepository.RemoveBook(id);
                    success = true;
                }
            }
            finally
            {
                DataLock.ExitWriteLock();
            }

            return success;
        }
        public BookDTO GetBookById(Guid id)
        {
            Book book;
            DataLock.EnterReadLock();

            try
            {
                book = DataRepository.GetBook(id);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            if (book != null)
                return new BookDTO { Id = id, Title = book.Title, Author = book.Author };
            else
                return null;

        }
        public BookDTO GetBookByTitle(string title)
        {
            BookDTO book = null;

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
                {
                    if (x.Value.Title == title)
                    {
                        book = new BookDTO() { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author };
                        break;
                    }
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return book;
        }
        public List<BookDTO> GetAllBooks()
        {
            List<BookDTO> bookList = new List<BookDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
                {
                    bookList.Add(new BookDTO { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return bookList;
        }
        public List<BookDTO> GetAvailableBooks()
        {
            List<BookDTO> bookList = new List<BookDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
                {
                    if (!IsBookLoanedNoLock(x.Key))
                        bookList.Add(new BookDTO { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return bookList;
        }
        public bool IsBookLoaned(Guid bookId)
        {
            bool result;

            DataLock.EnterReadLock();
            try
            {
                result = IsBookLoanedNoLock(bookId);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return result;
        }
        private bool IsBookLoanedNoLock(Guid bookId)
        {
            LoanDTO loan = GetLoanByBookNoLock(bookId);
            return loan != null;
        }

        public void AddNewReader(ReaderDTO reader)
        {
            DataLock.EnterWriteLock();
            try
            {
                DataRepository.AddReader(new Reader(reader.Name, 0, ""));
            }
            finally
            {
                DataLock.ExitWriteLock();
            }
        }
        public bool RemoveReaderById(Guid id)
        {
            bool success = false;

            DataLock.EnterWriteLock();
            try
            {
                List<LoanDTO> loans = GetAllLoansByReaderNoLock(id);
                if (loans.Count == 0)
                {
                    DataRepository.RemoveReader(id);
                    success = true;
                }
            }
            finally
            {
                DataLock.ExitWriteLock();
            }

            return success;
        }
        public ReaderDTO GetReaderById(Guid id)
        {
            Reader reader;

            DataLock.EnterReadLock();
            try
            {
                reader = DataRepository.GetReader(id);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            if (reader != null)
                return new ReaderDTO { Id = id, Name = reader.Name };
            else
                return null;
        }
        public ReaderDTO GetReaderByName(string name)
        {
            ReaderDTO reader = null;

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Reader> x in DataRepository.GetReaders())
                {
                    if (x.Value.Name == name)
                        reader = new ReaderDTO { Id = x.Key, Name = x.Value.Name };
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return reader;
        }
        public List<ReaderDTO> GetAllReaders()
        {
            List<ReaderDTO> readerList = new List<ReaderDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Reader> x in DataRepository.GetReaders())
                {
                    readerList.Add(new ReaderDTO { Id = x.Key, Name = x.Value.Name });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return readerList;
        }

        public bool LoanBook(Guid readerId, Guid bookId, DateTime now, DateTime returnDate)
        {
            bool success = false;

            DataLock.EnterWriteLock();
            try
            {
                Book book = DataRepository.GetBook(bookId);
                Reader reader = DataRepository.GetReader(readerId);
                if (book != null && reader != null && !IsBookLoanedNoLock(bookId))
                {
                    DataRepository.AddLoan(new Loan(bookId, readerId, now, returnDate));
                    success = true;
                }
            }
            finally
            {
                DataLock.ExitWriteLock();
            }

            return success;
        }
        public bool RemoveLoanById(Guid id)
        {
            bool success = false;

            DataLock.EnterWriteLock();
            try
            {
                Loan loan = DataRepository.GetLoan(id);
                if (loan != null)
                {
                    DataRepository.RemoveLoan(id);
                    success = true;
                }

            }
            finally
            {
                DataLock.ExitWriteLock();
            }

            return success;
        }
        public LoanDTO GetLoanById(Guid id)
        {
            Loan loan;

            DataLock.EnterReadLock();
            try
            {
                loan = DataRepository.GetLoan(id);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            if (loan != null)
                return new LoanDTO { Id = id, BookId = loan.BookId, ReaderId = loan.ReaderId, BorrowDate = loan.BorrowDate, ReturnDate = loan.ReturnDate };
            else
                return null;
        }
        public bool ReturnBook(Guid bookId)
        {
            bool success = false;

            DataLock.EnterWriteLock();
            try
            {
                foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
                {
                    if (x.Value.BookId == bookId)
                    {
                        DataRepository.RemoveLoan(x.Key);
                        success = true;
                        break;
                    }
                }
            }
            finally
            {
                DataLock.ExitWriteLock();
            }

            return success;
        }
        public List<LoanDTO> GetAllLoans()
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
                {
                    loanList.Add(new LoanDTO
                    {
                        Id = x.Key,
                        BookId = x.Value.BookId,
                        ReaderId = x.Value.ReaderId,
                        BorrowDate = x.Value.BorrowDate,
                        ReturnDate = x.Value.ReturnDate
                    });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return loanList;
        }
        public List<LoanDTO> GetAllLoansByReader(Guid readerId)
        {
            List<LoanDTO> loanList;

            DataLock.EnterReadLock();
            try
            {
                loanList = GetAllLoansByReaderNoLock(readerId);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return loanList;
        }
        private List<LoanDTO> GetAllLoansByReaderNoLock(Guid readerId)
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

            foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
            {
                if (x.Value.ReaderId == readerId)
                    loanList.Add(new LoanDTO
                    {
                        Id = x.Key,
                        BookId = x.Value.BookId,
                        ReaderId = x.Value.ReaderId,
                        BorrowDate = x.Value.BorrowDate,
                        ReturnDate = x.Value.ReturnDate
                    });
            }

            return loanList;
        }
        public LoanDTO GetLoanByBook(Guid bookId)
        {
            LoanDTO result;

            DataLock.EnterReadLock();
            try
            {
                result = GetLoanByBookNoLock(bookId);
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return result;
        }
        private LoanDTO GetLoanByBookNoLock(Guid bookId)
        {
            foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
            {
                if (x.Value.BookId == bookId)
                    return new LoanDTO
                    {
                        Id = x.Key,
                        BookId = x.Value.BookId,
                        ReaderId = x.Value.ReaderId,
                        BorrowDate = x.Value.BorrowDate,
                        ReturnDate = x.Value.ReturnDate
                    };
            }
            return null;
        }
        public List<LoanDTO> GetLoansBetweenDates(DateTime begin, DateTime end)
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
                {
                    if (x.Value.BorrowDate >= begin && x.Value.BorrowDate <= end)
                        loanList.Add(new LoanDTO
                        {
                            Id = x.Key,
                            BookId = x.Value.BookId,
                            ReaderId = x.Value.ReaderId,
                            BorrowDate = x.Value.BorrowDate,
                            ReturnDate = x.Value.ReturnDate
                        });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return loanList;
        }
        public List<LoanDTO> GetOverdueLoans(DateTime currentDate)
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

            DataLock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
                {
                    if (x.Value.ReturnDate < currentDate)
                        loanList.Add(new LoanDTO
                        {
                            Id = x.Key,
                            BookId = x.Value.BookId,
                            ReaderId = x.Value.ReaderId,
                            BorrowDate = x.Value.BorrowDate,
                            ReturnDate = x.Value.ReturnDate
                        });
                }
            }
            finally
            {
                DataLock.ExitReadLock();
            }

            return loanList;
        }

        public IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer)
        {
            return Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<List<LoanDTO>> observer)
        {
            OverdueObserverLock.EnterWriteLock();
            try
            {
                EventObservers.Add(observer);
            }
            finally
            {
                OverdueObserverLock.ExitWriteLock();
            }

            return new EventUnsubscriber(EventObservers, observer, OverdueObserverLock);
        }

        private class EventUnsubscriber: IDisposable
        {
            private readonly List<IObserver<List<LoanDTO>>> ObserverList;
            private readonly IObserver<List<LoanDTO>> Observer;
            private readonly ReaderWriterLockSlim Lock;
            public EventUnsubscriber(List<IObserver<List<LoanDTO>>> _observerList, IObserver<List<LoanDTO>> _observer, ReaderWriterLockSlim _lock)
            {
                ObserverList = _observerList;
                Observer = _observer;
                Lock = _lock;
            }
            public void Dispose()
            {
                Lock.EnterWriteLock();
                try
                {
                    ObserverList.Remove(Observer);
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public void CheckForOverdueLoans(object sender, ElapsedEventArgs args)
        {
            List<LoanDTO> loans = GetOverdueLoans(DateTime.Now);

            try
            {
                OverdueObserverLock.EnterReadLock();
                foreach (IObserver<List<LoanDTO>> observer in EventObservers)
                    observer.OnNext(loans);
            }
            finally
            {
                OverdueObserverLock.ExitReadLock();
            }

        }
    }
}
