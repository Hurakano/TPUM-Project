using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServer.ApplicationDataLayer
{
    public class LibraryRepository : ILibraryData
    {
        private readonly DataContext RepoData = new DataContext();

        public void FillData(IDataFiller filler)
        {
            filler.FillData(this);
        }

        public Guid AddBook(Book book)
        {
            Guid id = Guid.NewGuid();
            RepoData.Books.Add(id, book);
            return id;
        }

        public Book GetBook(Guid id)
        {
            if (RepoData.Books.ContainsKey(id))
                return RepoData.Books[id];
            else
                return null;
        }

        public Dictionary<Guid, Book> GetBooks() { return RepoData.Books; }

        public void UpdateBook(Guid id, Book book)
        {
            if (RepoData.Books.ContainsKey(id))
                RepoData.Books[id] = book;
        }

        public void RemoveBook(Guid id) { RepoData.Books.Remove(id); }

        public Guid AddReader(Reader reader)
        {
            Guid id = Guid.NewGuid();
            RepoData.Readers.Add(id, reader);
            return id;
        }

        public Reader GetReader(Guid id)
        {
            if (RepoData.Readers.ContainsKey(id))
                return RepoData.Readers[id];
            else
                return null;
        }

        public Dictionary<Guid, Reader> GetReaders() { return RepoData.Readers; }

        public void UpdateReader(Guid id, Reader reader)
        {
            if (RepoData.Readers.ContainsKey(id))
                RepoData.Readers[id] = reader;
        }

        public void RemoveReader(Guid id) { RepoData.Readers.Remove(id); }

        public Guid AddLoan(Loan loan)
        {
            Guid id = Guid.NewGuid();
            RepoData.Loans.Add(id, loan);
            return id;

        }

        public Loan GetLoan(Guid id)
        {
            if (RepoData.Loans.ContainsKey(id))
                return RepoData.Loans[id];
            else
                return null;
        }

        public Dictionary<Guid, Loan> GetLoans() { return RepoData.Loans; }

        public void UpdateLoan(Guid id, Loan loan)
        {
            if (RepoData.Loans.ContainsKey(id))
                RepoData.Loans[id] = loan;
        }

        public void RemoveLoan(Guid id) { RepoData.Loans.Remove(id); }
    }
}
