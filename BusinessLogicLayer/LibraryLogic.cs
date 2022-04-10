using System;
using System.Collections.Generic;
using System.Text;
using ApplicationDataLayer;

namespace BusinessLogicLayer
{
    public class LibraryLogic: IBusinessLogic
    {
        private readonly ILibraryData DataRepository;

        public LibraryLogic(ILibraryData repository)
        {
            DataRepository = repository;
        }

        public Book GetBookById(int id) { return DataRepository.GetBook(id); }
        public Book GetBookByTitle(string title)
        {
            foreach(Book x in DataRepository.GetBooks())
            {
                if (x.Title == title)
                    return x;
            }
            return null;
        }
        public List<Book> GetAllBooks() { return DataRepository.GetBooks(); }

        public Reader GetReaderById(int id) { return DataRepository.GetReader(id); }
        public Reader GetReaderByName(string name)
        {
            foreach(Reader x in DataRepository.GetReaders())
            {
                if (x.Name == name)
                    return x;
            }
            return null;
        }
        public List<Reader> GetAllReaders() { return DataRepository.GetReaders(); }

        public List<Loan> GetAllLoansByReader(Reader reader)
        {
            List<Loan> loans = new List<Loan>();

            foreach(Loan x in DataRepository.GetLoans())
            {
                if (x.Borrower == reader)
                    loans.Add(x);
            }
            return loans;
        }
    }
}
