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

        public Guid AddBook(string _title, DateTime _publicationDate, string _author)
        {
            Guid id = Guid.NewGuid();
            RepoData.Books.Add(id, new BookImpl(_title, _publicationDate, _author));
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

        public Guid AddReader(string _name, uint _age, string _address)
        {
            Guid id = Guid.NewGuid();
            RepoData.Readers.Add(id, new ReaderImpl(_name, _age, _address));
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

        public Guid AddLoan(Guid _bookId, Guid _readerId, DateTime _borrowDate, DateTime _returnDate)
        {
            Guid id = Guid.NewGuid();
            RepoData.Loans.Add(id, new LoanImpl(_bookId, _readerId, _borrowDate, _returnDate));
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
