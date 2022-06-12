using System;
using System.Collections.Generic;

namespace LibraryServer.ApplicationDataLayer
{
    public interface ILibraryData
    {
        void FillData(IDataFiller filler);

        Guid AddBook(string _title, DateTime _publicationDate, string _author);
        Book GetBook(Guid id);
        Dictionary<Guid, Book> GetBooks();
        void UpdateBook(Guid id, Book book);
        void RemoveBook(Guid id);

        Guid AddReader(string _name, uint _age, string _address);
        Reader GetReader(Guid id);
        Dictionary<Guid, Reader> GetReaders();
        void UpdateReader(Guid id, Reader reader);
        void RemoveReader(Guid id);

        Guid AddLoan(Guid _bookId, Guid _readerId, DateTime _borrowDate, DateTime _returnDate);
        Loan GetLoan(Guid id);
        Dictionary<Guid, Loan> GetLoans();
        void UpdateLoan(Guid id, Loan loan);
        void RemoveLoan(Guid id);
    }

    public static class LibraryDataFactory
    {
        public static ILibraryData CreateDataLayer()
        {
            return new LibraryRepository();
        }
    }
}
