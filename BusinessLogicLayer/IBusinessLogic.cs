using System;
using System.Collections.Generic;
using ApplicationDataLayer;

namespace BusinessLogicLayer
{
    public interface IBusinessLogic
    {
        Book GetBookById(int id);
        Book GetBookByTitle(string title);
        List<Book> GetAllBooks();

        Reader GetReaderById(int id);
        Reader GetReaderByName(string name);
        List<Reader> GetAllReaders();

        List<Loan> GetAllLoansByReader(Reader reader);
    }
}
