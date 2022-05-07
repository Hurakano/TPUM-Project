using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
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

        public ICommand OnBookReturn { get; set; }
        public ICommand OnBorrowBook { get; set; }

        public LibraryMainWindowVM()
        {
            DataModel = AbstractLibraryModel.GetModel();

            OnBookReturn = new CommandForwarder_P1<Guid>((id) => BookReturn(id));
            OnBorrowBook = new CommandForwarder(() => BorrowBook());

            DataModel.OverdueWatcher.OverdueEvent += OnOverdueEvent;

            foreach (ReaderPresenter reader in DataModel.GetReaders())
            {
                b_Readers.Add(reader);
            }
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
            RefreshReaderLoans();
            RefreshAvailableBooks();
            foreach (LoanPresenter loan in b_OverdueLoans)
            {
                if (loan.BookId == bookId)
                {
                    b_OverdueLoans.Remove(loan);
                    OnPropertyChanged("OverdueLoans");
                    break;
                }
            }
        }

        public void BorrowBook()
        {
            if (b_SelectedBook != null && b_SelectedReader != null && b_ReturnTime.HasValue)
            {
                DataModel.BorrowBook(b_SelectedReader.Id, b_SelectedBook.Id, b_ReturnTime.Value);
                b_SelectedBook = null;
                RefreshAvailableBooks();
                RefreshReaderLoans();
            }
        }

        private void RefreshReaderLoans()
        {
            if (b_SelectedReader != null)
            {
                b_LoansOfReader.Clear();
                foreach (LoanPresenter loan in DataModel.GetLoansByReader(b_SelectedReader.Id))
                {
                    b_LoansOfReader.Add(loan);
                }
                OnPropertyChanged("ReaderLoans");
            }
        }

        private void RefreshAvailableBooks()
        {
            b_Books.Clear();
            foreach (BookPresenter book in DataModel.GetAvailableBooks())
            {
                b_Books.Add(book);
            }
            OnPropertyChanged("AvailableBooks");
        }
    }
}
