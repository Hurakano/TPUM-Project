using System;
using System.Collections.Generic;

namespace ApplicationDataLayer
{
    public interface ILibraryData
    {
        void AddBook(Book book);
        Book GetBook(int id);
        List<Book> GetBooks();
        void UpdateBook(int id, Book book);
        void RemoveBook(int id);

        void AddReader(Reader reader);
        Reader GetReader(int id);
        List<Reader> GetReaders();
        void UpdateReader(int id, Reader reader);
        void RemoveReader(int id);

        void AddLoan(Loan loan);
        Loan GetLoan(int id);
        List<Loan> GetLoans();
        void UpdateLoan(int id, Loan loan);
        void RemoveLoan(int id);
    }
}
