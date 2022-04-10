using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public class Loan
    {
        public Book BorrowedBook;
        public Reader Borrower;
        public DateTime BorrowDate;
        public DateTime ReturnDate;

        public Loan(Book _book, Reader _reader, DateTime _borrowDate, DateTime _returnDate)
        {
            BorrowedBook = _book;
            Borrower = _reader;
            BorrowDate = _borrowDate;
            ReturnDate = _returnDate;
        }
    }
}
