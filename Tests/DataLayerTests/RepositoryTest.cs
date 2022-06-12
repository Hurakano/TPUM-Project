using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServer.ApplicationDataLayer.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestAddBook()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string bookTitle1 = "Title1";
            string bookTitle2 = "Title2";
            string bookAuthor1 = "Author1";
            string bookAuthor2 = "Author2";
            DateTime bookDate = new DateTime(2010, 5, 5, 0, 0, 0);

            Guid bookId1 = repository.AddBook(bookTitle1, bookDate, bookAuthor1);
            Guid bookId2 = repository.AddBook(bookTitle2, new DateTime(2015, 1, 4, 0, 0, 0), bookAuthor2);

            Dictionary<Guid, Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 2);
            Assert.AreEqual(booksIn[bookId2].Author, bookAuthor2);

            Book bookCheck = repository.GetBook(bookId1);
            Assert.IsNotNull(bookCheck);
            Assert.AreEqual(bookCheck.Title, bookTitle1);
            Assert.AreEqual(bookCheck.PublicationDate, bookDate);
        }

        [TestMethod]
        public void TestRemoveBook()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string bookAuthor2 = "Author2";

            Guid bookId1 = repository.AddBook("Title1", new DateTime(2010, 5, 5, 0, 0, 0), "Author1");
            repository.AddBook("Title2", new DateTime(2015, 1, 4, 0, 0, 0), bookAuthor2);

            repository.RemoveBook(bookId1);

            Dictionary<Guid, Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 1);
            Assert.IsNull(repository.GetBook(bookId1));
        }

        [TestMethod]
        public void TestAddReader()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string name1 = "Name1";
            string name2 = "Name2";
            uint age1 = 20;
            uint age2 = 50;
            string address1 = "Address1";
            string address2 = "Address2";

            Guid readerId1 = repository.AddReader(name1, age1, address1);
            Guid readerId2 = repository.AddReader(name2, age2, address2);

            Dictionary<Guid, Reader> readers = repository.GetReaders();
            Assert.AreEqual(readers.Count, 2);
            Assert.AreEqual(readers[readerId2].Address, address2);

            Reader readerCheck = repository.GetReader(readerId1);
            Assert.IsNotNull(readerCheck);
            Assert.AreEqual(readerCheck.Name, name1);
            Assert.AreEqual(readerCheck.Age, age1);
        }

        [TestMethod]
        public void TestRemoveReader()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string readerName2 = "Name2";

            Guid readerId1 = repository.AddReader("Name1", 20, "Address1");
            repository.AddReader(readerName2, 30, "Address2");

            repository.RemoveReader(readerId1);

            Dictionary<Guid, Reader> readersIn = repository.GetReaders();
            Assert.AreEqual(readersIn.Count, 1);
            Assert.IsNull(repository.GetReader(readerId1));
        }

        [TestMethod]
        public void TestAddLoan()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();

            Guid bookId = repository.AddBook("Title", new DateTime(2000), "Author");
            Guid readerId = repository.AddReader("Name", 20, "Address");

            DateTime date1 = new DateTime(2005);
            DateTime date2 = new DateTime(2010);
            Guid loanId = repository.AddLoan(bookId, readerId, date1, date2);

            Dictionary<Guid, Loan> loans = repository.GetLoans();
            Assert.AreEqual(loans.Count, 1);
            Assert.AreEqual(loans[loanId].BookId, bookId);
            Assert.AreEqual(loans[loanId].ReaderId, readerId);
            Assert.AreEqual(loans[loanId].BorrowDate, date1);
            Assert.AreEqual(loans[loanId].ReturnDate, date2);
        }

        [TestMethod]
        public void TestRemoveLoan()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();

            Guid bookId = repository.AddBook("Title", new DateTime(2000), "Author");
            Guid readerId = repository.AddReader("Name", 20, "Address");

            Guid loanId = repository.AddLoan(bookId, readerId, new DateTime(2005), new DateTime(2010));

            Loan loanCheck = repository.GetLoan(loanId);
            Assert.IsNotNull(loanCheck);

            repository.RemoveLoan(loanId);
            Dictionary<Guid, Loan> loans = repository.GetLoans();
            Assert.AreEqual(loans.Count, 0);
            Assert.IsNull(repository.GetLoan(loanId));
        }

        [TestMethod]
        public void TestDataFiller()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            IDataFiller filler = new ApplicationDataLayer.ExampleFiller();

            repository.FillData(filler);

            Dictionary<Guid, Book> books = repository.GetBooks();
            Dictionary<Guid, Reader> readers = repository.GetReaders();
            Dictionary<Guid, Loan> loans = repository.GetLoans();

            Assert.AreEqual(books.Count, 5);
            Assert.AreEqual(readers.Count, 3);
            Assert.AreEqual(loans.Count, 3);

            Assert.AreEqual(readers.ElementAt(0).Value.Name, "Jan Kowalski");
        }
    }
}
