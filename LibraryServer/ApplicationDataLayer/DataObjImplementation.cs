using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServer.ApplicationDataLayer
{
    class BookImpl: Book
    {
        public BookImpl(string _title, DateTime _publicationDate, string _author): base(_title, _publicationDate, _author)
        {
            
        }
    }

    class ReaderImpl: Reader
    {
        public ReaderImpl(string _name, uint _age, string _address): base(_name, _age, _address)
        {

        }
    }

    class LoanImpl: Loan
    {
        public LoanImpl(Guid _bookId, Guid _readerId, DateTime _borrowDate, DateTime _returnDate): base(_bookId, _readerId, _borrowDate, _returnDate)
        {

        }
    }
}
