using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServer.ApplicationDataLayer
{
    public abstract class Loan
    {
        public Guid BookId;
        public Guid ReaderId;
        public DateTime BorrowDate;
        public DateTime ReturnDate;

        public Loan(Guid _bookId, Guid _readerId, DateTime _borrowDate, DateTime _returnDate)
        {
            BookId = _bookId;
            ReaderId = _readerId;
            BorrowDate = _borrowDate;
            ReturnDate = _returnDate;
        }
    }
}
