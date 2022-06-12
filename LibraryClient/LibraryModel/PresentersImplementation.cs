using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.LibraryModel
{
    class BookPresenterImpl: BookPresenter
    {
        override public string TitleWithAuthor 
        {
            get => Title + ", " + Author;
        }

        public BookPresenterImpl(Guid id, string _title, string _author): base(id, _title, _author)
        {

        }
    }

    class ReaderPresenterImpl: ReaderPresenter
    {
        public ReaderPresenterImpl(Guid _id, string _name): base(_id, _name)
        {

        }
    }

    class LoanPresenterImpl: LoanPresenter
    {
        public LoanPresenterImpl(string _bookTitle, string _readerName, Guid bookId, Guid readerId, DateTime returnDate):
            base(_bookTitle, _readerName, bookId, readerId, returnDate)
        {

        }
    }
}
