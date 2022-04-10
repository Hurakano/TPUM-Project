using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public class LibraryRepository : ILibraryData
    {
        private readonly DataContext RepoData = new DataContext();

        public void AddBook(Book book) => RepoData.Books.Add(book);

        public Book GetBook(int id) { return RepoData.Books[id]; }

        public List<Book> GetBooks() { return RepoData.Books; }

        public void UpdateBook(int id, Book book) => RepoData.Books[id] = book;

        public void RemoveBook(int id) => RepoData.Books.RemoveAt(id);

        public void AddReader(Reader reader) => RepoData.Readers.Add(reader);

        public Reader GetReader(int id) { return RepoData.Readers[id]; }

        public List<Reader> GetReaders() { return RepoData.Readers; }

        public void UpdateReader(int id, Reader reader) => RepoData.Readers[id] = reader;

        public void RemoveReader(int id) => RepoData.Readers.RemoveAt(id);

        public void AddLoan(Loan loan) => RepoData.Loans.Add(loan);

        public Loan GetLoan(int id) { return RepoData.Loans[id]; }

        public List<Loan> GetLoans() { return RepoData.Loans; }

        public void UpdateLoan(int id, Loan loan) => RepoData.Loans[id] = loan;

        public void RemoveLoan(int id) => RepoData.Loans.RemoveAt(id);
    }
}
