using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Timers;
using PresentationLayer.LibraryModel;

namespace PresentationLayer.LibraryViewModel
{
    public class LibraryMainWindowVM : LibraryViewModelBase
    {
        private ObservableCollection<ReaderPresenter> b_Readers = new ObservableCollection<ReaderPresenter>();
        private ReaderPresenter b_SelectedReader = null;
        private ObservableCollection<LoanPresenter> b_LoansOfReader = new ObservableCollection<LoanPresenter>();
        private ObservableCollection<LoanPresenter> b_OverdueLoans = new ObservableCollection<LoanPresenter>();
        private ObservableCollection<BookPresenter> b_Books = new ObservableCollection<BookPresenter>();
        private BookPresenter b_SelectedBook = null;
        private Nullable<DateTime> b_ReturnTime;
        private bool BookBorrowOngoing = false;
        private bool BookReturnOngoing = false;

        public ICommand OnBookReturn { get; set; }
        public ICommand OnBorrowBook { get; set; }

        public LibraryMainWindowVM()
        {
            DataModel = AbstractLibraryModel.GetModel();

            OnBookReturn = new CommandForwarder_P1<Guid>((id) => BookReturn(id));
            OnBorrowBook = new CommandForwarder(() => BorrowBook());

            DataModel.OverdueWatcher.OverdueEvent += OnOverdueEvent;
            DataModel.OnDataUpdated += DataUpdatedHandler;
            DataModel.OnTransactionResult += OnTransactionResult;

            RefreshReaders();
            RefreshAvailableBooks();
        }

        public ObservableCollection<ReaderPresenter> Readers
        {
            get { return b_Readers; }
            set
            {
                if (!value.Equals(b_Readers))
                {
                    b_Readers = value;
                    OnPropertyChanged("Readers");
                }
            }
        }

        public ReaderPresenter SelectedReader
        {
            get { return b_SelectedReader; }
            set
            {
                b_SelectedReader = value;
                OnPropertyChanged("SelectedReader");
                RefreshReaderLoans();
            }
        }

        public ObservableCollection<LoanPresenter> ReaderLoans
        {
            get { return b_LoansOfReader; }
            set
            {
                if (!value.Equals(b_LoansOfReader))
                {
                    b_LoansOfReader = value;
                    OnPropertyChanged("ReaderLoans");
                }
            }
        }

        public BookPresenter SelectedBook
        {
            get { return b_SelectedBook; }
            set
            {
                b_SelectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        public ObservableCollection<LoanPresenter> OverdueLoans
        {
            get { return b_OverdueLoans; }
            set
            {
                b_OverdueLoans = value;
                OnPropertyChanged("OverdueLoans");
            }
        }

        public ObservableCollection<BookPresenter> AvailableBooks
        {
            get { return b_Books; }
            set
            {
                b_Books = value;
                OnPropertyChanged("AvailableBooks");
            }
        }

        private string b_TransactionResultInfo = "";
        public string TransactionResultInfo
        {
            get { return b_TransactionResultInfo; }
            set
            {
                b_TransactionResultInfo = value;
                OnPropertyChanged("TransactionResultInfo");
            }
        }
        private string b_TransactionResultColor = "White";
        public string TransactionResultColor
        {
            get { return b_TransactionResultColor; }
            set
            {
                b_TransactionResultColor = value;
                OnPropertyChanged("TransactionResultColor");
            }
        }

        public Nullable<DateTime> ReturnTime
        {
            get { return b_ReturnTime; }
            set
            {
                b_ReturnTime = value;
                OnPropertyChanged("BorrowPeriod");
            }
        }

        private void OnOverdueEvent(object sender, EventArgs e)
        {
            OverdueLoansEventArgs args = (OverdueLoansEventArgs)e;
            ObservableCollection<LoanPresenter> newLoans = new ObservableCollection<LoanPresenter>();
            foreach (Guid loanId in args.LoanIds)
            {
                newLoans.Add(DataModel.GetLoanById(loanId));
            }

            b_OverdueLoans = newLoans;
            OnPropertyChanged("OverdueLoans");
        }

        public void BookReturn(Guid bookId)
        {
            DataModel.ReturnBook(bookId);
            BookReturnOngoing = true;
        }

        public void BorrowBook()
        {
            if (b_SelectedBook != null && b_SelectedReader != null && b_ReturnTime.HasValue)
            {
                DataModel.BorrowBook(b_SelectedReader.Id, b_SelectedBook.Id, b_ReturnTime.Value);
                BookBorrowOngoing = true;
            }
        }

        private void RefreshReaderLoans()
        {
            if (b_SelectedReader != null)
            {
                ObservableCollection<LoanPresenter> loans = new ObservableCollection<LoanPresenter>();

                foreach (LoanPresenter loan in DataModel.GetLoansByReader(b_SelectedReader.Id))
                {
                    loans.Add(loan);
                }

                b_LoansOfReader = loans;
                OnPropertyChanged("ReaderLoans");
            }
        }

        private void RefreshAvailableBooks()
        {
            ObservableCollection<BookPresenter> books = new ObservableCollection<BookPresenter>();

            foreach (BookPresenter book in DataModel.GetAvailableBooks())
            {
                books.Add(book);
            }

            b_Books = books;
            OnPropertyChanged("AvailableBooks");
        }

        private void RefreshReaders()
        {
            ObservableCollection<ReaderPresenter> readers = new ObservableCollection<ReaderPresenter>();

            foreach (ReaderPresenter reader in DataModel.GetReaders())
            {
                readers.Add(reader);
            }

            b_Readers = readers;
            OnPropertyChanged("Readers");
        }

        private void DataUpdatedHandler(object sender, int args)
        {
            switch(args)
            {
                case 1:
                    RefreshReaders();
                    break;
                case 2:
                    RefreshAvailableBooks();
                    RefreshReaderLoans();
                    break;
            }
        }

        private void OnTransactionResult(object sender, bool success)
        {
            if(success)
            {
                if(BookReturnOngoing)
                {
                    BookReturnOngoing = false;
                    RefreshReaderLoans();
                    RefreshAvailableBooks();
                    ObservableCollection<LoanPresenter> overdueLoans = new ObservableCollection<LoanPresenter>(b_OverdueLoans);
                    foreach (LoanPresenter loan in overdueLoans)
                    {
                        if (DataModel.GetAvailableBooks().Find(x => x.Id == loan.BookId) != null)
                        {
                            overdueLoans.Remove(loan);
                            b_OverdueLoans = overdueLoans;
                            OnPropertyChanged("OverdueLoans");
                            break;
                        }
                    }

                    TransactionResultInfo = "Book return success";
                    TransactionResultColor = "LightGreen";
                }
                else if(BookBorrowOngoing)
                {
                    BookBorrowOngoing = false;
                    b_SelectedBook = null;
                    RefreshAvailableBooks();
                    RefreshReaderLoans();

                    TransactionResultInfo = "Book borrow success";
                    TransactionResultColor = "LightGreen";
                }
            }
            else
            {
                if(BookReturnOngoing)
                {
                    BookReturnOngoing = false;
                    TransactionResultInfo = "Book return failed";
                    TransactionResultColor = "Red";
                }
                else if(BookBorrowOngoing)
                {
                    BookBorrowOngoing = false;
                    TransactionResultInfo = "Book borrow failed";
                    TransactionResultColor = "Red";
                }
            }

            System.Timers.Timer resultCloseTimer = new System.Timers.Timer(TimeSpan.FromSeconds(5).TotalMilliseconds)
            {
                AutoReset = false
            };
            resultCloseTimer.Elapsed += new ElapsedEventHandler((sender, args) => { TransactionResultInfo = ""; TransactionResultColor = "White"; });
            resultCloseTimer.Start();
        }
    }
}
