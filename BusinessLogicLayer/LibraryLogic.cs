using System;
using System.Collections.Generic;
using ApplicationDataLayer;
using System.Timers;

namespace BusinessLogicLayer
{
    public class LibraryLogic: IBusinessLogic, IObservable<List<LoanDTO>>
    {
        private readonly ILibraryData DataRepository;
        private readonly List<IObserver<List<LoanDTO>>> EventObservers;
        private readonly Timer EventTimer;

        public LibraryLogic(ILibraryData repository)
        {
            DataRepository = repository;
            EventObservers = new List<IObserver<List<LoanDTO>>>();
            EventTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds)
            {
                AutoReset = true
            };
            EventTimer.Elapsed += new ElapsedEventHandler(CheckForOverdueLoans);
            EventTimer.Start();

        }

        public static IBusinessLogic Create(int repositorySelect)
        {
            ILibraryData data;

            switch (repositorySelect)
            {
                default:
                case 0:
                    data = new LibraryRepository();
                    break;
                case 1:
                    data = new LibraryRepository();
                    data.FillData(new ExampleFiller());
                    break;
            }

            return new LibraryLogic(data);
        }

        public void AddNewBook(BookDTO book)
        {
            DataRepository.AddBook(new Book(book.Title, new DateTime(), book.Author));
        }
        public void RemoveBookById(Guid id)
        {
            if (!IsBookLoaned(id))
                DataRepository.RemoveBook(id);
            
        }
        public BookDTO GetBookById(Guid id)
        {
            Book book = DataRepository.GetBook(id);
            if (book != null)
                return new BookDTO { Id = id, Title = book.Title, Author = book.Author };
            else
                return null;

        }
        public BookDTO GetBookByTitle(string title)
        {
            foreach(KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
            {
                if (x.Value.Title == title)
                    return new BookDTO() { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author };
            }
            return null;
        }
        public List<BookDTO> GetAllBooks()
        {
            List<BookDTO> bookList = new List<BookDTO>();

            foreach (KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
            {
                bookList.Add(new BookDTO { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author });
            }

            return bookList;
        }
        public List<BookDTO> GetAvailableBooks()
        {
            List<BookDTO> bookList = new List<BookDTO>();

            foreach (KeyValuePair<Guid, Book> x in DataRepository.GetBooks())
            {
                if(!IsBookLoaned(x.Key))
                    bookList.Add(new BookDTO { Id = x.Key, Title = x.Value.Title, Author = x.Value.Author });
            }

            return bookList;
        }
        public bool IsBookLoaned(Guid bookId)
        {
            LoanDTO loan = GetLoanByBook(bookId);
            return loan != null;
        }

        public void AddNewReader(ReaderDTO reader)
        {
            DataRepository.AddReader(new Reader(reader.Name, 0, ""));
        }
        public void RemoveReaderById(Guid id)
        {
            List<LoanDTO> loans = GetAllLoansByReader(id);

            if (loans.Count == 0)
                DataRepository.RemoveReader(id);
        }
        public ReaderDTO GetReaderById(Guid id)
        {
            Reader reader = DataRepository.GetReader(id);

            if (reader != null)
                return new ReaderDTO { Id = id, Name = reader.Name };
            else
                return null;
        }
        public ReaderDTO GetReaderByName(string name)
        {
            foreach(KeyValuePair<Guid, Reader> x in DataRepository.GetReaders())
            {
                if (x.Value.Name == name)
                    return new ReaderDTO { Id = x.Key, Name = x.Value.Name };
            }
            return null;
        }
        public List<ReaderDTO> GetAllReaders()
        {
            List<ReaderDTO> readerList = new List<ReaderDTO>();

            foreach (KeyValuePair<Guid, Reader> x in DataRepository.GetReaders())
            {
                readerList.Add(new ReaderDTO { Id = x.Key, Name = x.Value.Name });
            }

            return readerList;
        }

        public void LoanBook(Guid readerId, Guid bookId, DateTime now, DateTime returnDate)
        {
            Book book = DataRepository.GetBook(bookId);
            Reader reader = DataRepository.GetReader(readerId);
            if (book != null && reader != null && !IsBookLoaned(bookId))
                DataRepository.AddLoan(new Loan(bookId, readerId, now, returnDate));
        }
        public void RemoveLoanById(Guid id) { DataRepository.RemoveLoan(id); }
        public LoanDTO GetLoanById(Guid id)
        {
            Loan loan = DataRepository.GetLoan(id);
            if (loan != null)
                return new LoanDTO { Id = id, BookId = loan.BookId, ReaderId = loan.ReaderId, BorrowDate = loan.BorrowDate, ReturnDate = loan.ReturnDate };
            else
                return null;
        }
        public void ReturnBook(Guid bookId)
        {
            foreach (KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
            {
                if (x.Value.BookId == bookId)
                {
                    DataRepository.RemoveLoan(x.Key);
                    break;
                }
            }
        }
        public List<LoanDTO> GetAllLoans()
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

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

            return loanList;
        }
        public List<LoanDTO> GetAllLoansByReader(Guid readerId)
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

            foreach(KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
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
            foreach(KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
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
            
            foreach(KeyValuePair<Guid, Loan> x in DataRepository.GetLoans())
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

            return loanList;
        }
        public List<LoanDTO> GetOverdueLoans(DateTime currentDate)
        {
            List<LoanDTO> loanList = new List<LoanDTO>();

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

            return loanList;
        }

        public IDisposable SubscribeToOverdueEvent(IObserver<List<LoanDTO>> observer)
        {
            return Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<List<LoanDTO>> observer)
        {
            EventObservers.Add(observer);

            return new EventUnsubscriber(EventObservers, observer);
        }

        private class EventUnsubscriber: IDisposable
        {
            private readonly List<IObserver<List<LoanDTO>>> ObserverList;
            private readonly IObserver<List<LoanDTO>> Observer;
            public EventUnsubscriber(List<IObserver<List<LoanDTO>>> _observerList, IObserver<List<LoanDTO>> _observer)
            {
                ObserverList = _observerList;
                Observer = _observer;
            }
            public void Dispose()
            {
                ObserverList.Remove(Observer);
            }
        }

        public void CheckForOverdueLoans(object sender, ElapsedEventArgs args)
        {
            List<LoanDTO> loans = GetOverdueLoans(DateTime.Now);

            foreach (IObserver<List<LoanDTO>> observer in EventObservers)
                observer.OnNext(loans);
        }
    }
}
