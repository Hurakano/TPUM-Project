using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogicLayer;

namespace PresentationLayer.LibraryModel
{
    public class LoanPresenter
    {
        public string BookTitle { get; set; }
        public Guid BookId { get; private set; }
        public string ReaderName { get; set; }
        public Guid ReaderId { get; private set; }
        public string ReturnDateText { get; set; }
        public DateTime ReturnDate { get; set; }

        public LoanPresenter(string _bookTitle, string _readerName, Guid bookId, Guid readerId, DateTime returnDate)
        {
            BookTitle = _bookTitle;
            ReaderName = _readerName;
            BookId = bookId;
            ReaderId =readerId;
            ReturnDate = returnDate;
            ReturnDateText = ReturnDate.ToString("dd/MM/yyyy");
        }
    }
}
