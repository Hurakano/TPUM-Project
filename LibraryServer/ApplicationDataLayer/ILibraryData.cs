using System;
using System.Collections.Generic;

namespace LibraryServer.ApplicationDataLayer
{
    public interface ILibraryData
    {
        void FillData(IDataFiller filler);

        Guid AddBook(Book book);
        Book GetBook(Guid id);
        Dictionary<Guid, Book> GetBooks();
        void UpdateBook(Guid id, Book book);
        void RemoveBook(Guid id);

        Guid AddReader(Reader reader);
        Reader GetReader(Guid id);
        Dictionary<Guid, Reader> GetReaders();
        void UpdateReader(Guid id, Reader reader);
        void RemoveReader(Guid id);

        Guid AddLoan(Loan loan);
        Loan GetLoan(Guid id);
        Dictionary<Guid, Loan> GetLoans();
        void UpdateLoan(Guid id, Loan loan);
        void RemoveLoan(Guid id);
    }
}
